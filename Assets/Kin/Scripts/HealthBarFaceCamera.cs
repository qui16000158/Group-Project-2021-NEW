using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBarFaceCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);

        transform.eulerAngles += new Vector3(0, 180, 0);
    }
}
