using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSelector : NetworkBehaviour
{
    
    [ClientRpc]
    public void RpcSpawn(Vector3 position)
    {
        CharacterController cc = GetComponent<CharacterController>();

        if (cc && cc.enabled)
        {
            cc.enabled = false;
        }

        transform.position = position;

        if (cc && !cc.enabled)
        {
            cc.enabled = true;
        }
    }
}
