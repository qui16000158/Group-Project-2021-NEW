using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class EggCollectible : NetworkBehaviour
{
    EggCount eggCounter;
    Material eggMat;
    
    private void Awake()
    {
        eggCounter = GameObject.FindGameObjectWithTag("EggCount").GetComponent<EggCount>();

        eggMat = transform.GetChild(1).GetComponent<Material>();

        eggMat.color = new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f));
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
