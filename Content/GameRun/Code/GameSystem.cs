using UnityEngine;
using UnityEngine.SceneManagement;
public class GameRun_GameSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject GUI;
    [SerializeField] private GameObject GameClearGUI;
    [SerializeField] private GameObject GameOverGUI;
    public static void GameClearEvent()
    {
        SceneManager.LoadScene("Goal");
        // ManagerPause.Instance.Pause();
        // GameClearGUI.SetActive(true);
    }
    public static void GameOverEvent()
    {
        SceneManager.LoadScene("GameOver");
        // ManagerPause.Instance.Pause();
        // GameOverGUI.SetActive(true);
    }
}