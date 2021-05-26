using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

public class EggCount : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateEggUI))]
    int eggsFound = 0;

    [SyncVar]
    int eggsInLevel = 5;

    public UnityEvent LevelComplete;

    public TMP_Text EggText;
    public TMP_Text EndEggText;
    public GameObject EndLevelUI;

    private void Start()
    {
        CheckEggNumber();
        UpdateEggUI(0, eggsFound);
    }

    void CheckEggNumber()
    {
        if (isServer)
        {

        }
    }

    public void PickUpEgg()
    {
        eggsFound += 1;

        UpdateEggUI(0, eggsFound);
    }

    void UpdateEggUI(int oldValue, int newValue)
    {
        EggText.text = newValue + "/" + eggsInLevel;
    }

    public void EndLevel()
    {
        EndLevelUI.SetActive(true);
        EndEggText = EggText;
    }
}
