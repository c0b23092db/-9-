using UnityEngine;

public class commonKomorebi : MonoBehaviour
{
    [SerializeField] private GameObject komorebi;
    [SerializeField] private Vector2 揺れの大きさ = new Vector2(10f, 10f);
    [SerializeField] private float timeSpeed = 1.0f;

    private void Update()
    {
        float time = Time.time * timeSpeed;
        float x = Mathf.Sin(time) * 揺れの大きさ.x;
        float y = Mathf.Cos(time) * 揺れの大きさ.y;

        komorebi.transform.localPosition = new Vector3(x, y, 0f);
    }
}