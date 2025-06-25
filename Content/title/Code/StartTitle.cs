using UnityEngine;
using UnityEngine.SceneManagement;
public class StartTitle : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            ManagerGameSystem.Instance.isClickTitle = true;
            SceneManager.LoadScene("Title");
        }
    }
}