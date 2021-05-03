using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTouch : NetworkBehaviour
{
    [SerializeField]
    UnityEvent OnTouchEvent;
    [SerializeField]
    new string tag = "Player";

    // Run an event if touching an object with the correct tag
    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.tag == tag)
            {
                OnTouchEvent.Invoke();
            }
        }
    }
}
