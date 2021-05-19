using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EggCount : NetworkBehaviour
{
    [SyncVar]
    int eggsFound = 0;

    int eggsInLevel = 5;

    public void PickUpEgg()
    {
        eggsFound += 1;

    }

    private void Update()
    {
        print(eggsFound);
    }
}
