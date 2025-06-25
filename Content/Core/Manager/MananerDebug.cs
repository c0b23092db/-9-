using UnityEngine;
public class ManagerDebug : MonoBehaviour
{
    public static ManagerDebug Instance { get; private set; }
    public static bool adminPlay { get; private set; } = true;
    public static bool DebugPlay = true;
    public static bool DebugLog = false;
}