using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public static void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
        SceneManager.UnloadSceneAsync("FrontEnd");
    }

    public static void UnloadMainScene()
    {
        SceneManager.UnloadSceneAsync("Main");
        SceneManager.LoadScene("FrontEnd");
    }
}
