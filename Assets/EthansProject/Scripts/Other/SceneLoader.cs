using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    public string sceneToLoad;

    Slider loadingBar;

    // Use this for initialization
    void Start()
    {
        loadingBar = GetComponent<Slider>();
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// Loads the next sccene in build index asynchronously
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            float loadProgress = Mathf.Clamp01(ao.progress / 0.9f);
            // assign the loading bar value for load progress visualisation
            loadingBar.value = loadProgress;
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }


}
