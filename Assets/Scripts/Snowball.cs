using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : NetworkBehaviour
{
    [SerializeField]
    GameObject iceBlockPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.tag == "Water")
            {
                Vector3 pos = transform.position;

                Destroy(gameObject);

                GameObject iceBlock = Instantiate(iceBlockPrefab, pos, Quaternion.identity);

                NetworkServer.Spawn(iceBlock);
            }
        }
    }
}
