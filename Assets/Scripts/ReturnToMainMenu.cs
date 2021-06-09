using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // When the player presses escape, release the mouse and return them to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Mirror creates these objects by default and forces them to persist between scenes
            // We remove them because we want to end the current session
            GameObject networkManager = GameObject.Find("NetworkManager");

            GameObject debugUpdater = GameObject.Find("[Debug Updater]");
            if (networkManager)
            {
                NetworkManager.Shutdown();
                Destroy(networkManager);
            }
            if(debugUpdater)
                Destroy(debugUpdater);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Starting");
        }
    }
}
