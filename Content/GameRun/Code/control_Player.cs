using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class control_Player : MonoBehaviour
{
    [Header("移動設定")]

    [SerializeField] private float maxSpeed = 10.0f;      // 最高速度
    [SerializeField] private float accelerationTimeSpeed = 2.0f;  // 加速時間
    [SerializeField] private float decelerationTimeSpeed = 1.5f;  // 減速時間
    private Vector2 currentVelocity = Vector2.zero;
    private Rigidbody2D currentRigidbody;

    [Header("回転設定")]
    [SerializeField] private bool smoothRotation = true;      // 滑らかな回転をするかどうか
    [SerializeField] private float rotation = 360.0f;    // 回転速度（度/秒）

    [Header("ジャンプ設定")]
    [SerializeField] private float maxJumpPower = 15.0f;      // 最大ジャンプ力
    [SerializeField] private float accelerationTimeChargeJump = 1.0f;   // ジャンプチャージ時間
    [SerializeField] private float accelerationChargeScale = 0.9f; // チャージ時最小スケール加速時間
    [SerializeField] private float maxJumpTime = 1.0f; // ジャンプの最大時間
    [SerializeField] private float maxScale = 1.2f; // ジャンプ時最大スケール加速時間
    [SerializeField] private float maxJumpAngle = 45.0f; // ジャンプの最大角度
    private bool isCharging = false;
    private bool isJumping = false;
    private float chargedJumpPower = 0.0f;
    private float chargedJumpTime = 0.0f;
    private float useJumpTime = 0.00f;
    private float percentJumpPower = 0.0f;

    [Header("体力設定")]
    [SerializeField] float playerHpRecoverySpeed = 0.1f;   // 体力回復速度

    // プレイヤー関連 //
    private int maxHp = 100;
    private int playerHp = 100;
    private bool isPutLight = false;
    private bool isDamege = false;

    // スケール関連 //
    private Vector3 originScale;
    void Start()
    {
        currentRigidbody = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
        accelerationTimeSpeed *= maxSpeed;
        decelerationTimeSpeed *= maxSpeed;
        accelerationTimeChargeJump = maxJumpPower / accelerationTimeChargeJump;
        InvokeRepeating("Apply_Recovery", 0.0f, playerHpRecoverySpeed);
    }

    void Update()
    {
        Input_Debug(ManagerDebug.DebugPlay);
        if (ManagerPause.IsPaused) return;
        if (playerHp <= 0 && !ManagerDebug.DebugPlay) GameRun_GameSystem.GameOverEvent();
        if (!isJumping)
        {
            Input_Movement();
            Input_Jump();
        }
        else
            Update_Jump();
        Update_Movement();
    }
    void OnGUI()
    {
        onGUI_Debug(ManagerDebug.DebugPlay);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (ManagerDebug.DebugLog)
            Debug.Log("衝突 - OnTriggerEnter2D");
        if (collision.tag == "Goal")
        {
            if (ManagerDebug.DebugLog)
                Debug.Log("Goal!");
            GameRun_GameSystem.GameClearEvent();
        }
        if (collision.tag == "Enemy_ground")
                isDamege = true;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Light_field")
            isPutLight = true;
        // Debug.Log("衝突 - OnTriggerStay2D");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light_field")
            isPutLight = false;
        if (ManagerDebug.DebugLog)
            Debug.Log("衝突 - OnTriggerExit2D");
    }
    private void Apply_Recovery()
    {
        if (isJumping) return;
        if (isPutLight)
        {
            if (playerHp < maxHp)
                playerHp++;
        }
        else if (0 < playerHp)
            playerHp--;
    }

    private Vector2 Input_Movement()
    {
        // 入力の取得 //
        Vector2 inputDirection = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            inputDirection.y += 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            inputDirection.y -= 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            inputDirection.x -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            inputDirection.x += 1;
        // 加速・減速の処理 //
        if (inputDirection != Vector2.zero)
        { // 入力がある場合：目標速度に向かって加速
            Vector2 targetVelocity = inputDirection * maxSpeed;
            if (isCharging) // チャージ中は移動速度を半分に
                targetVelocity *= 0.5f;
            if(!isPutLight) // 光を踏んでいないときは移動速度を半分に
                targetVelocity *= 0.5f;
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, accelerationTimeSpeed * Time.deltaTime);
        }
        else
        { // 入力がない場合：減速
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, decelerationTimeSpeed * Time.deltaTime);
        }
        Update_Rotation(inputDirection.normalized);

        return currentVelocity;
    }

    private void Input_Jump()
    {
        percentJumpPower = Mathf.Clamp01(chargedJumpPower / maxJumpPower);
        float percentHp = (float)playerHp / maxHp;
        if (Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.isPressed)
        {
            isCharging = true;
            // マウス方向を向く //
            Vector3 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            if (mouseDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (chargedJumpPower < maxJumpPower * percentHp)
            {
                transform.localScale = Vector2.Lerp(originScale, originScale * accelerationChargeScale, percentJumpPower);
                chargedJumpPower += accelerationTimeChargeJump * Time.deltaTime;
            }
            if (maxJumpPower * percentHp < chargedJumpPower)
                chargedJumpPower = maxJumpPower * percentHp;
        }

        if ((Keyboard.current.spaceKey.wasReleasedThisFrame || Mouse.current.leftButton.wasReleasedThisFrame) && isCharging)
        {
            chargedJumpTime = 0.0f;
            // ジャンプ時間を計算 //
            useJumpTime = maxJumpTime * percentJumpPower;
            // ジャンプ方向の計算 //
            float angleRad = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 jumpDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            currentVelocity += jumpDirection * chargedJumpPower;
            chargedJumpPower = 0.0f;
            isCharging = false;
            isJumping = true;
        }
    }

    void Update_Jump()
    {
        float changedScale = Mathf.Max(1.0f, maxScale * percentJumpPower);
        if (useJumpTime < chargedJumpTime)
        {
            isJumping = false;
            currentVelocity *= 0.5f;
            transform.localScale = originScale;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z);
            return;
        }
        chargedJumpTime += Time.deltaTime;
        // if (chargedJumpTime < useJumpTime / 2)
        // {
        //     // 放物線に飛ぶ物体を撮るカメラの近さを考慮したサイズ変更 //
        //     transform.localScale = Vector2.Lerp(transform.localScale, originalScale * changedScale, chargedJumpTime / (useJumpTime / 2));
        //     // 放物線に飛ぶ物体の角度を逐次変更 //
        //     transform.rotation = Quaternion.Lerp(Quaternion.Euler(0.0f, maxJumpAngle * percentJumpPower, transform.rotation.eulerAngles.z), Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z), chargedJumpTime / useJumpTime);
        // }
        // else if (useJumpTime / 2 < chargedJumpTime)
        // {
        //     transform.localScale = Vector2.Lerp(originalScale * changedScale, originalScale, (chargedJumpTime - (useJumpTime / 2)) / (useJumpTime / 2));
        //     transform.rotation = Quaternion.Lerp(Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z), Quaternion.Euler(0.0f, -1 * maxJumpAngle * percentJumpPower, transform.rotation.eulerAngles.z), chargedJumpTime / useJumpTime);
        // }
        float halfTime = useJumpTime / 2f; // AI出力 // ここから //
        float t = Mathf.Clamp01(chargedJumpTime / halfTime);
        float easedT = Mathf.SmoothStep(0f, 1f, t);
        if (chargedJumpTime < halfTime)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, originScale * changedScale, easedT);
            float angleY = Mathf.Lerp(maxJumpAngle * percentJumpPower, 0f, easedT);
            transform.rotation = Quaternion.Euler(0f, angleY, transform.rotation.eulerAngles.z);
        }
        else
        {
            float tBack = Mathf.Clamp01((chargedJumpTime - halfTime) / halfTime);
            float easedBackT = Mathf.SmoothStep(0f, 1f, tBack);
            transform.localScale = Vector2.Lerp(originScale * changedScale, originScale, easedBackT);
            float angleY = Mathf.Lerp(0f, -maxJumpAngle * percentJumpPower, easedBackT);
            transform.rotation = Quaternion.Euler(0f, angleY, transform.rotation.eulerAngles.z);
        } // AI出力 // ここまで //
    }

    private void Update_Movement()
    {
        transform.position += (Vector3)currentVelocity * Time.deltaTime;
        // currentRigidbody.position += currentVelocity * Time.deltaTime;
    }

    private void Update_Rotation(Vector2 inputDirection)
    {
        if (inputDirection == Vector2.zero) return;
        float targetAngle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
        if (smoothRotation) // 滑らかな回転
            transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotation * Time.deltaTime));
        else // 即座に回転
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    void Input_Debug(bool Trigger = false)
    {
        if (ManagerDebug.adminPlay && Keyboard.current.f1Key.wasPressedThisFrame)
            ManagerDebug.DebugPlay = !ManagerDebug.DebugPlay;
        if (Trigger)
        {
            if (Keyboard.current.f2Key.wasPressedThisFrame)
                ManagerDebug.DebugLog = !ManagerDebug.DebugLog;
            if (Keyboard.current.digit8Key.isPressed)
                playerHp--;
            if (Keyboard.current.digit9Key.isPressed)
                playerHp++;
            if (Keyboard.current.pKey.isPressed)
                transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime);
            if (Keyboard.current.oKey.isPressed)
                transform.Rotate(new Vector3(0, -10, 0) * Time.deltaTime);
            if (Keyboard.current.lKey.isPressed)
                transform.localScale += new Vector3(0.1f, 0.1f, 0f);
            if (Keyboard.current.kKey.isPressed)
                transform.localScale -= new Vector3(0.1f, 0.1f, 0f);
        }
    }
    private void onGUI_Debug(bool Trigger = false)
    {
        if (Trigger)
        { // 変数をGUIで表示 //
            GUI.Label(new Rect(10, 10, 200, 20), $"Velocity: {currentVelocity.x:F2}, {currentVelocity.y:F2}");
            GUI.Label(new Rect(10, 20, 200, 20), $"HP: {playerHp}/{maxHp}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Jump Charge: {chargedJumpPower:F1}/{maxJumpPower:F1}/{maxJumpPower*(float)playerHp/maxHp:F1}");
            GUI.Label(new Rect(10, 40, 200, 20), $"Jumping Timer: {chargedJumpTime:F2}/{useJumpTime:F2}");
        }
    }

}