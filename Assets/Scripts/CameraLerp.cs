using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : NetworkBehaviour
{
    [SerializeField]
    new Transform camera;
    [SerializeField]
    Transform cameraAnchor;

    // Start is called before the first frame update
    void Start()
    {
        // This script should only exist for the local player controlling the camera
        if (isLocalPlayer)
        {
            camera.gameObject.SetActive(true);
            camera.SetParent(null); // Detach the camera from the anchor
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 lastPos = camera.position;
        Vector3 wantedPos = cameraAnchor.position;

        Quaternion lastAng = camera.rotation;
        Quaternion wantedAng = cameraAnchor.rotation;

        camera.position = Vector3.Slerp(lastPos, wantedPos, 0.1f);
        camera.rotation = Quaternion.Slerp(lastAng, wantedAng, 0.1f);
    }
}
