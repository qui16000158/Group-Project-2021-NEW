using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    // A public method which allows us to load scenes from the UnityEvent inspector
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    // This will exit the game.
    public void Quit()
    {
        Application.Quit();
    }

    // This will load a scene after a short delay (A public method allows it to be set in the inspector)
    public void LoadSceneDelayed(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    // This will load a scene after a short delay
    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
