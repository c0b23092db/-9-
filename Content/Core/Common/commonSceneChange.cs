using UnityEngine;
using UnityEngine.SceneManagement;
public class commonSceneChange : MonoBehaviour
{
    public static void BackTitle()
    {
        if (Input.anyKey)
            SceneManager.LoadScene("Title");
    }
}