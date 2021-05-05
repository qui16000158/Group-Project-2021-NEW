using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBarFaceCamera : MonoBehaviour
{
    Camera cachedCamera;

    void Update()
    {
        if (!cachedCamera)
        {
            cachedCamera = Camera.main;
        }
        else
        {
            transform.LookAt(cachedCamera.transform);

            transform.eulerAngles += new Vector3(0, 180, 0);
        }
    }
}
