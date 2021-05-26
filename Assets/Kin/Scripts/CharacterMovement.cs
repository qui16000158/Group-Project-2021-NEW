using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovement : NetworkBehaviour
{
    public CharacterController characterController;
    //public GameObject playerCamera;
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

    bool pauseMovement = false;

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

    public void PausePlayerMovement(bool pause)
    {
        if (pause)
        {
            pauseMovement = true;
        }
        else
        {
            pauseMovement = false;
        }
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            if(TryGetComponent(out CharacterController cc))
            {
                Destroy(cc);
            }
            return;
        }

        jumpSpeed = Mathf.Sqrt(-2 * gravity * jumpHeight); 
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitnormal = hit.normal;
        print(hitnormal);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        xDir = Input.GetAxisRaw("Horizontal");
        zDir = Input.GetAxisRaw("Vertical");

        InputDirection = new Vector3(xDir, 0f, zDir).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputted = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (characterController == null) return;
        if (pauseMovement) return;

        //Move Direction
        float targetAngle = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float targetTurnAngle = Mathf.Atan2(lastInputDirection.x, lastInputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        float currentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetTurnAngle, ref turnSmoothVelocity, turnSmoothTime);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        moveDirection = moveDirection.normalized;

        transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);

        if (characterController.isGrounded)
        {
            verticalVelocity = -10f;
        }
        else if (verticalVelocity > jumpSpeed/3 && !Input.GetKey(KeyCode.Space))
        {
            verticalVelocity = jumpSpeed / 3;
        }
        else if (verticalVelocity > 0)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            if (verticalVelocity > maxFallSpeed)
            {
                verticalVelocity += gravity * gravityModifier * Time.deltaTime;
            }
            else
            {
                verticalVelocity = maxFallSpeed;
            }
        }

        Jump();

        if (InputDirection.magnitude >= 0.3f)
        {
            if (new Vector2(currentVelocity.x, currentVelocity.z).magnitude < targetSpeed)
            {
                moveSpeed += acceleration;
                

                if (new Vector2(currentVelocity.x, currentVelocity.z).magnitude > targetSpeed)
                {
                    moveSpeed = targetSpeed;
                }
            }
        }
        else
        {
            moveSpeed -= Mathf.Min(Mathf.Abs(moveSpeed), friction);
        }

        currentVelocity = moveSpeed * moveDirection;

        //Slide();

        currentVelocity += (Vector3.up * verticalVelocity);

        characterController.Move(currentVelocity * Time.deltaTime);
  
    }

    void Slide()
    {
        groundAngle = Vector3.Angle(Vector3.up, hitnormal);

        if (groundAngle > 90)
        {
            groundAngle = 90 - (groundAngle - 90);
        }

        if (characterController.isGrounded && groundAngle > 0.5f)
        {

            currentVelocity += hitnormal * groundAngle / 90 * slideSpeed;
            verticalVelocity -= groundAngle / 90 * slideSpeed;
        }
    }

    void Jump()
    {
        if (jumpInputted)
        {
            if (characterController.isGrounded)
            {
                //characterController.Move(jumpyVelocity * Vector3.up);
                verticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            }

            jumpInputted = false;
        }
    }

}