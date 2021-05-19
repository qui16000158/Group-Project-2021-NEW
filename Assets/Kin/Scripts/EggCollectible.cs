using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class EggCollectible : NetworkBehaviour
{

    public int healAmount = 50;

    void OnTriggerEnter(Collider other)
    {

    }
}
