using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Button : MonoBehaviour {

    /// <summary>
    /// Loads the scene.
    /// 
    /// If the paramiter is empty the next scene in the build index is loadeds
    /// else, the scene with the param as its name is loaded.
    /// </summary>
    /// <param name="_SceneName">]The name of scene to load.</param>
    public void LoadScene(string _SceneName)
    {
        if (_SceneName == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(_SceneName);
        }
       
    }
}
