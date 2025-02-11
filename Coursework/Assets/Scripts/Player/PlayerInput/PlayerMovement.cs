using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    // controlls how fast the player moves
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;

    // desiredMoveSpeed handles momentum
    private float desiredMoveSpeed;
    private float lastdesiredMoveSpeed;

    // 
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    // applies drag to the player when grounded to reduce speed realistically
    public float groundDrag;

    [Header("Jumping")]
    // variable to apply physics to allow player to jump
    public float jumpforce;
    // cooldown for jumping
    public float jumpCooldown;
    // controls physics in air
    public float airMultiplier;
    // boolean that will check if cooldown for jump is over
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    //public KeyCode CjumpKey = KeyCode.JoystickButton11;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Grounded")]
    // float to store a value of how high a player is
    public float playerHeight;
    // layer mask to assign to indicate what the floor is
    public LayerMask WhatisGround;
    // boolean to check if player is grounded to then apply specific variables 
    public bool grounded;

    [Header("slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    // get the movement values of orientation
    public Transform orientation;

    // will take values from "w,a,s,d"
    float horizontalInput;
    float verticalInput;

    // vector3 variable to control movement direction
    Vector3 moveDirection;

    // variable to referece the rigid body
    Rigidbody rb;

    // defines the state that the player is in to apply correct physics
    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air,
    }

    public bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // stops the player from falling over
        rb.freezeRotation = true;

        canJump = true;

        // saves default value of y for player to reference 
        startYScale = transform.localScale.y;
    }

    public void Update()
    {
        // deploys a racast downwards from the player if it is hitting an object with a "whatisground" layermask
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatisGround);

        MyInput();
        moveSpeedControl();
        StateHandler();

        // handles drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // gets wasd inputs and stores then in a variable
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // if pressed jump, checks if space is pressed, cd is over and on the ground already.
        if(Input.GetKey(jumpKey) && canJump && grounded)
        {
            // sets it to false so player can't jump constantly 
            canJump = false;
            // calls jump function 
            Jump();
            // allows player to continuously jump within set parameters
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouching
        if (Input.GetKeyDown(crouchKey))
        {
            // shrinks the player down 
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            // adds force as shrinking the player down doesn't affect their percieved height
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            // returns player to default size
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    // defines the state the player is in based on parameters
    private void StateHandler()
    {
        // mode - sliding
        if (sliding)
        {
            state = MovementState.sliding;
            if(onSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }

        // mode - crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // mode - sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // mode - walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // mode - air
        else
        {
            state = MovementState.air;
        }

        //check if desiredMoveSpeed has changed drastically and if it has slowly increase speed to desired speed to create a sense of momentum
        if (Mathf.Abs(desiredMoveSpeed - lastdesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastdesiredMoveSpeed = desiredMoveSpeed;
    }

    // changes move speed -> desired move speed over time
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movement speed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time/difference);

            // the steeper the slop the more accelleration the player gets
            if(onSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier; 
            }

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }



    private void MovePlayer()
    {
        //calculate movement direction
        // code makes sure you always walk in the direction you are facing
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (onSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //adds force to the movement if on ground
        else if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //applies force if not on the groun
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
        // turn gravity off while on slope
        rb.useGravity = !onSlope();
    }

    private void moveSpeedControl()
    {
        // limit speed on slope
        if (onSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // if statment to limit velocity if needed.
            if (flatVel.magnitude > moveSpeed)
            {
                // calculates what max velocity should be 
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                // applies limited velocity to rigid body
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        //reset y velocity to insure jump height is consistent
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);

        rb.AddForce(Vector3.down * 200f, ForceMode.Force);
    }

    private void ResetJump()
    {
        canJump = true;

        exitingSlope = false;
    }

    public bool onSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
