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

    new Transform camera;

    private void Start()
    {
        camera = Camera.main.transform;
    }

    [Command]
    void CmdThrow(Vector3 spawnPos, Vector3 normal)
    {
        GameObject snowBall = Instantiate(snowBallPrefab, spawnPos, Quaternion.identity);
        NetworkServer.Spawn(snowBall);
        snowBall.GetComponent<Rigidbody>().velocity = normal * throwStrength;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButtonDown("Fire1"))
        {
            CmdThrow(hand.position, camera.forward);
        }
    }
}
