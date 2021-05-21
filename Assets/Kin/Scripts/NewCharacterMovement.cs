using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;

    Vector3 currentVelocity;
    float moveSpeed = 0;
    public float targetSpeed = 8f;
    public float acceleration = 1.5f;
    public float friction = 4f;

    bool jumpInputted = false;
    float verticalVelocity;
    float jumpHeight = 4f;
    float jumpSpeed;
    float gravity = -50f;
    float gravityModifier = 1.4f;
    float maxFallSpeed = -40;

    float xDir;
    float zDir;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 hitnormal;
    float groundAngle;
    float slideSpeed = 20f;

    Vector3 lastInputDirection;
    Vector3 _InputDirection;
    Vector3 InputDirection
    {
        get { return _InputDirection; }

        set
        {
            if (value != Vector3.zero)
            {
                lastInputDirection = _InputDirection;
            }

            _InputDirection = value;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
