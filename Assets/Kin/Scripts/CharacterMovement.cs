using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMovement : NetworkBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;

    Vector3 currentVelocity;
    float topVelocity = 8f;

    bool jumpInputted = false;
    float verticalVelocity;
    float jumpHeight = 5f;
    float jumpSpeed;
    float gravity = -70f;
    float gravityModifier = 2.3f;



    /*
    Vector3 movementVelocity = Vector3.zero;
    float topMovementSpeed = 4f;
    float groundAcceleration = 2f;
    float groundDecceleration = 3f;
    float airAcceleration = 2f;
    float airDecceleration = 3f;
    */

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

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
        if (!isLocalPlayer)
        {
            if(TryGetComponent(out CharacterController cc))
            {
                Destroy(cc);
            }
            return;
        }

        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        jumpSpeed = Mathf.Sqrt(-2 * gravity * jumpHeight); 
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float xDir = Input.GetAxisRaw("Horizontal");
        float zDir = Input.GetAxisRaw("Vertical");

        InputDirection = new Vector3(xDir, 0f, zDir).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputted = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

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
        else if (verticalVelocity > jumpSpeed/4 && !Input.GetKey(KeyCode.Space))
        {
            verticalVelocity = jumpSpeed / 4;
            print("short jump");
        }
        else if (verticalVelocity > 0)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            if (verticalVelocity > -60)
            {
                verticalVelocity += gravity * gravityModifier * Time.deltaTime;
            }
            else
            {
                verticalVelocity = -60;
            }
        }

        Jump();

        if (InputDirection.magnitude >= 0.3f)
        {
            currentVelocity = (moveDirection * topVelocity);
        }
        else
        {
            currentVelocity = Vector3.zero;
        }

        currentVelocity += (Vector3.up * verticalVelocity);
        characterController.Move(currentVelocity * Time.deltaTime);

  
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
/*        if (InputDirection.magnitude >= 0.3f)
        {
            if (movementVelocity.magnitude <= topMovementSpeed)
            {
                movementVelocity = moveDirection * (movementVelocity.magnitude + groundAcceleration);

            }
            else
            {
                movementVelocity = moveDirection * topMovementSpeed;
            }

            currentVelocity += movementVelocity;
        }
        
        if (currentVelocity.magnitude > 0)
        {
            currentVelocity = Mathf.(currentVelocity - (currentVelocity.normalized * groundDecceleration));
        }


----


        if (InputDirection.magnitude >= 0.3f)
        {
            movementVelocity = moveDirection * topVelocity;
        }
        else
        {
            movementVelocity = Vector3.zero;
        }

        currentVelocity += movementVelocity;
        characterController.Move(currentVelocity * Time.deltaTime);
*/