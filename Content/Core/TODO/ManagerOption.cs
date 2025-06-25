using UnityEngine;
public class ManagerOption : MonoBehaviour
{
    public static ManagerOption Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

}