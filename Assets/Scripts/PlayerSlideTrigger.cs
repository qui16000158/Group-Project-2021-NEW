using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideTrigger : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer) return;

        if(other.TryGetComponent(out SlideJump slide))
        {
            slide.StartSlide(transform);
        }
    }
}
