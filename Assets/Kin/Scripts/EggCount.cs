using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Mirror;

public class EggCount : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateEggUI))]
    public int eggsFound = 0;

    [SyncVar]
    public int eggsInLevel = 5;

    public UnityEvent LevelComplete;

    public TMP_Text EggText;
    public TMP_Text EndEggText;
    public GameObject EndLevelUI;

    AudioSource eggSound;

    private void Start()
    {
        CheckEggNumber();
        UpdateEggUI(0, eggsFound);
        eggSound = GetComponent<AudioSource>();
    }

    void CheckEggNumber()
    {
        if (isServer)
        {
            eggsInLevel = GameObject.FindGameObjectsWithTag("Egg").Length;
        }
    }

    public void PickUpEgg()
    {
        eggsFound += 1;
        eggSound.Play();
        UpdateEggUI(0, eggsFound);
    }

    void UpdateEggUI(int oldValue, int newValue)
    {
        EggText.text = newValue + "/" + eggsInLevel;
    }

    public void UpdateEggUI()
    {
        EggText.text = eggsFound + "/" + eggsInLevel;
    }

    public void EndLevel()
    {
        EndLevelUI.SetActive(true);
        EndEggText = EggText;
    }
}
