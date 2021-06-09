﻿using System.Collections;
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

    public void LoadSceneDelayed(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
