using UnityEngine;

public class control_Camera : MonoBehaviour
{
    private Camera mainCamera;
    private float defaultSize;
    [SerializeField] private bool followCamera = true;
    [SerializeField] private Transform player;
    [SerializeField] private float FollorSpeed = 5.0f;
    [SerializeField] private float FollorSpeedOnPause = 5.0f;
    private Vector3 offset = new Vector3(0, 0, -10);
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        defaultSize = mainCamera.orthographicSize;
    }

    private void LateUpdate()
    {
        if (ManagerPause.IsPaused)
        {
            float t = 1 - Mathf.Exp(-FollorSpeedOnPause * Time.unscaledDeltaTime);
            transform.position = Vector3.Lerp(transform.position, player.position + offset, t);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, defaultSize * 0.8f, t);
            return;
        }
        else
        {
            float t = 1 - Mathf.Exp(-FollorSpeedOnPause * Time.deltaTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, defaultSize, t);
        }
        if (followCamera)
            transform.position = Vector3.Lerp(transform.position,player.position + offset,FollorSpeed * Time.deltaTime);
        else
            transform.position = player.transform.position + offset;
    }
}