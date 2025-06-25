using UnityEngine;
public class ManagerGameSystem : MonoBehaviour
{
    public static ManagerGameSystem Instance { get; private set; }
    public bool isClickTitle = false;
    public int numStage = 0;
    public bool isGamePlay = false;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

}