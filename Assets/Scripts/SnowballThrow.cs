using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballThrow : NetworkBehaviour
{
    [SerializeField]
    GameObject snowBallPrefab;
    [SerializeField]
    Transform hand; // The player's hand position
    [SerializeField]
    float throwStrength = 10.0f;
    float nextThrow = 0.0f;

    new Transform camera;

    private void Start()
    {
        camera = Camera.main.transform;
        Physics.IgnoreLayerCollision(9, 10); // Ignore collisions between players and snowballs
    }

    [Command]
    void CmdThrow(Vector3 spawnPos, Vector3 normal)
    {
        if (Time.time < nextThrow) return;

        nextThrow = Time.time + 1.5f;

        GameObject snowBall = Instantiate(snowBallPrefab, spawnPos, Quaternion.identity);
        NetworkServer.Spawn(snowBall);
        snowBall.GetComponent<Rigidbody>().velocity = normal * throwStrength;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButtonDown("Fire1"))
        {
            CmdThrow(hand.position, transform.forward);
        }
    }
}
