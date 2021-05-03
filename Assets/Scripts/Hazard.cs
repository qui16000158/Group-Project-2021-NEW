using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hazard : NetworkBehaviour
{
    [SerializeField]
    UnityEvent OnHazardTriggered;

    [SerializeField]
    int damage = 25;

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                OnHazardTriggered.Invoke();
                damageable.TakeDamage(damage);
            }
        }
    }
}
