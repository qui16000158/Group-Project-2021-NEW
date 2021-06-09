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
            else if(other.tag == "Enemy")
            {
                Vector3 pos = other.transform.position;

                Destroy(gameObject);

                GameObject iceBlock = Instantiate(iceBlockPrefab, pos, Quaternion.identity);

                other.transform.parent = iceBlock.transform;

                other.GetComponent<Hazard>().enabled = false;
                other.GetComponent<Animator>().enabled = false;

                NetworkServer.Spawn(iceBlock);
            }
            else if(other.tag == "Player")
            {
                Physics.IgnoreCollision(other, GetComponent<Collider>());
            }
        }
    }
}
