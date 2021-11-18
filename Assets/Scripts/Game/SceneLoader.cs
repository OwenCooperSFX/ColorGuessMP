using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public void LoadMainScene()
    {
        StartCoroutine(LoadSceneWithDelay("FrontEnd", "Main", .3f));
    }

    public void UnloadMainScene()
    {
        StartCoroutine(LoadSceneWithDelay("Main", "FrontEnd", .3f));
    }

    private IEnumerator LoadSceneWithDelay(string currentScene, string nextScene, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(nextScene);
    }
}
