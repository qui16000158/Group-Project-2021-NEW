using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class FishCollectible : NetworkBehaviour
{
    public int healAmount = 50;

    void OnTriggerEnter(Collider other)
    {
        if (isServer && other.GetType() != typeof(CharacterController))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.AddHealth(healAmount);
                Destroy(this.gameObject);
            }
        }
    }
}
