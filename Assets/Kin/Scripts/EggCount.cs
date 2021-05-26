using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

public class EggCount : NetworkBehaviour
{
    [SyncVar]
    int eggsFound = 0;

    int eggsInLevel = 5;

    public UnityEvent LevelComplete;

    public TMP_Text EggText;
    public TMP_Text EndEggText;
    public GameObject EndLevelUI;

    private void Start()
    {
        UpdateEggUI();
    }

    public void PickUpEgg()
    {
        eggsFound += 1;

        UpdateEggUI();
    }

    void UpdateEggUI()
    {
        EggText.text = eggsFound.ToString() + "/" + eggsInLevel.ToString();
    }

    public void EndLevel()
    {
        EndLevelUI.SetActive(true);
        EndEggText = EggText;
    }
}
