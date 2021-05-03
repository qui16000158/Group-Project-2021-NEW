using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplayBar : NetworkBehaviour, IHealthDisplay
{
    [SerializeField]
    Image healthBar;

    public void UpdateDisplay(Health health)
    {
        SetWidth(health.HP / (float)health.MaxHP);
    }

    [ClientRpc]
    public void SetWidth(float width)
    {
        if (isClient)
        {
            healthBar.fillAmount = width;
        }
    }
}
