using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    Vector3 spawnPoint;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position; // The initial spawn point is where the player originally spawned

        TryGetComponent(out cc); // Attempt to cache the player's character controller
    }

    // Respawn the player
    [ClientRpc]
    public void RpcRespawn()
    {
        // Character controllers interfere with transform.position, so it must be disabled beforehand

        if(cc != null)
        {
            cc.enabled = false;
        }

        transform.position = spawnPoint;

        if(cc != null)
        {
            cc.enabled = true;
        }
    }

    // if the player touches a checkpoint, update their spawn point
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Checkpoint")
        {
            spawnPoint = col.transform.position;
        }
    }
}
