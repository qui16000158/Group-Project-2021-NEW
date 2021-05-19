using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class EggCollectible : NetworkBehaviour
{
    EggCount eggCounter;
    
    private void Awake()
    {
        eggCounter = GameObject.FindGameObjectWithTag("EggCount").GetComponent<EggCount>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer && other.GetType() != typeof(CharacterController))
        {
            eggCounter.PickUpEgg();
            Destroy(this.gameObject);
        }

    }
}
