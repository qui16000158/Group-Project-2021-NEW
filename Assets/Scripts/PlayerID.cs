using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : NetworkBehaviour
{
    [SyncVar]
    public int ID;
}
