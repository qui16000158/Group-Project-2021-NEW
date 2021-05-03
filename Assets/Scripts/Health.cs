using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Health : NetworkBehaviour, IDamageable
{
    [SyncVar]
    public int MaxHP = 100;
    [SyncVar]
    public int HP;

    [SerializeField]
    UnityEvent OnDeath;

    [SerializeField]
    UnityEvent OnTakeDamage;

    IHealthDisplay[] healthDisplays = null;

    void Start()
    {
        if (isServer)
        {
            healthDisplays = GetComponents<IHealthDisplay>(); // Attempt to cache health display components

            HP = MaxHP;

            UpdateHealthDisplay();
        }
    }

    public void UpdateHealthDisplay()
    {
        if (healthDisplays == null) return; // Do not update health display if it is not valid
        // Loop through, and update all health displays
        foreach(IHealthDisplay healthDisplay in healthDisplays)
        {
            healthDisplay.UpdateDisplay(this);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isServer)
        {
            HP = Mathf.Max(0, HP - amount); // Will return 0, if damage would have put health below 0

            UpdateHealthDisplay(); // Update the on-screen health display

            // Necessary events are called below here
            OnTakeDamage.Invoke();

            if (HP == 0)
            {
                OnDeath.Invoke();
            }
        }
    }

    public int GetHealth()
    {
        return HP;
    }
#region EDITOR
#if UNITY_EDITOR
    [ContextMenu("Take 25 damage")]
    void TakeDamage25()
    {
        TakeDamage(25);
    }

    [ContextMenu("Take 5 damage")]
    void TakeDamage5()
    {
        TakeDamage(5);
    }

    [ContextMenu("Take 1 damage")]
    void TakeDamage1()
    {
        TakeDamage(1);
    }
#endif
#endregion EDITOR
}
