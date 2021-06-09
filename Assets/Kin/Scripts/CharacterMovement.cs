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

    float landingJumpBufferTimePeriod = 0.15f;
    float coyoteJumpTimePeriod = 0.2f;
    float landingJumpBufferTimer = 0;
    float coyoteJumpTimer = 0;
    bool blockCoyoteJump = false;

    float xDir;
    float zDir;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 hitnormal;
    float groundAngle;
    float slideSpeed = 20f;

    public Animator playerAnim;

    AudioSource playerSounds;
    public AudioClip walkSFX;
    public AudioClip jumpSFX;

    public ParticleSystem landingParticles;

    bool pauseMovement = false;

    bool _IsOnGround;
    bool IsOnGround
    {
        get { return _IsOnGround; }

        set
        {
            if (_IsOnGround == value)
            {
                return;
            }
            
            if (value == false)
            {
                coyoteJumpTimer = coyoteJumpTimePeriod;
                landingParticles.Play();
            }
            else if (value == true)
            {
                blockCoyoteJump = false;

                landingParticles.Play();
            }

            _IsOnGround = value;
        }
    }

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

        playerSounds = GetComponent<AudioSource>();
        StartCoroutine(WalkSoundLoop());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitnormal = hit.normal;
        //print(hitnormal);
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

    void HandleTimers()
    {
        IsOnGround = characterController.isGrounded;

        if (coyoteJumpTimer > 0)
        {
            coyoteJumpTimer -= Time.deltaTime;
        }
        else
        {
            coyoteJumpTimer = 0;
        }

        if (landingJumpBufferTimer > 0)
        {
            landingJumpBufferTimer -= Time.deltaTime;
        }
        else
        {
            landingJumpBufferTimer = 0;
        }
    }

    void HandleAnimations()
    {
        if (InputDirection.magnitude > 0.3f)
        {
            playerAnim.SetBool("walking", true);
        }
        else
        {
            playerAnim.SetBool("walking", false);
        }

        if (!characterController.isGrounded)
        {
            playerAnim.SetBool("inAir", true);
        }
        else
        {
            playerAnim.SetBool("inAir", false);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (characterController == null) return;
        if (pauseMovement) return;

        HandleTimers();
        HandleAnimations();

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

    IEnumerator WalkSoundLoop()
    {
        while (true)
        {
            if (InputDirection.magnitude >= 0.3f
                && characterController.isGrounded)
            {
                playerSounds.PlayOneShot(walkSFX);
            }

            yield return new WaitForSeconds(0.25f);
        }
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

                playerSounds.PlayOneShot(jumpSFX);

                blockCoyoteJump = true;
            }
            else if (coyoteJumpTimer > 0 && !blockCoyoteJump)
            {
                verticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                playerSounds.PlayOneShot(jumpSFX);
                coyoteJumpTimer = 0;
            }
            else
            {
                landingJumpBufferTimer = landingJumpBufferTimePeriod;
            }

            jumpInputted = false;
        }

        if (landingJumpBufferTimer > 0 
            && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            playerSounds.PlayOneShot(jumpSFX);
            landingJumpBufferTimer = 0;
        }
    }

}