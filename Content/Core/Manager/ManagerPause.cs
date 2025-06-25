using UnityEngine;
public class ManagerPause : MonoBehaviour
{
    public static ManagerPause Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public static bool IsPaused { get; private set; } = false;

    public void PauseOrResume()
    {
        if (Time.timeScale == 1.0f)
            Pause();
        else
            Resume();
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
        IsPaused = true;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        IsPaused = false;
    }
}