using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnClient : NetworkBehaviour
{
    [SerializeField]
    Animator anim;

    // Using update ensures all clients will execute this script when spawning
    void Update()
    {
        if (!isServer)
        {
            anim.enabled = false;
            enabled = false;
        }
    }
}
