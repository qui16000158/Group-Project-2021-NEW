using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevelDisplayEggCount : MonoBehaviour
{
    public TMP_Text endEggCountText;
    EggCount eggCount;

    void Start()
    {
        eggCount = FindObjectOfType<EggCount>();

        endEggCountText.text = eggCount.eggsFound + "/" + eggCount.eggsInLevel;
    }
}
