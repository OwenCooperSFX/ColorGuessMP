using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public void LoadMainScene()
    {
        StartCoroutine(LoadSceneWithDelay("Main", .3f));
    }

    public void UnloadMainScene()
    {
        StartCoroutine(LoadSceneWithDelay("FrontEnd", .3f));
    }

    private IEnumerator LoadSceneWithDelay(string nextScene, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadSceneAsync(nextScene);
    }
}
