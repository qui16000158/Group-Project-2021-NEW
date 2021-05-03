using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class HealthDisplayText : NetworkBehaviour, IHealthDisplay
{
    [SerializeField]
    TMP_Text textDisplay;

    public void UpdateDisplay(Health health)
    {
        RpcSetText(health.HP);
    }

    [ClientRpc]
    void RpcSetText(int HP)
    {
        if (isClient)
        {
            textDisplay.text = "" + HP;
        }
    }
}
