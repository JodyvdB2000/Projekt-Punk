using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementScript : AbstractAction
{
    #region Live Action
    public enum PlayerState
    {
        run,
        dash,
        crouch,
        slide,
        air,
        wallRun,
        wallClimb,
    }

    [Header("Live Actions")]

    [Tooltip("What the player's most recent action was.")]
    public PlayerState state;
    private PlayerState lastState;
    #endregion

    [Space(15)]

    public float horizontalInput;
    public float verticalInput;

    [Header("Running")]
    [Header("Active Speeds")]
    [Tooltip("Running speed (probably gonna be the default movement speed).")]
    public float moveSpeed;
    public float desiredMoveSpeed;
    public float lastDesiredMoveSpeed;
    public float maxYSpeed;

    [Space(10)]

    [Header("Stored Speeds")]
    public float wallRunSpeed;
    public float runSpeed;
    public float crouchedSpeed;
    public float dashSpeed;
    public float slideSpeed;
    public float airMinSpeed;

    [Space(10)]

    [Header("Change Factors")]
    private float speedChangeFactor;
    [Space(5)]
    public bool keepMomentum;
    [Space(5)]
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float dashIncreaseMultiplier;
    public float airborneIncreaseMultiplier;
    public float uncrouchIncreaseMultiplier;

    [Space(10)]

    [Header("Live Values")]
    public bool moving;
    public float currentVelocity;
    [HideInInspector] public Vector3 moveDirection;
    private Vector3 slopedDirection;

    [Space(10)]

    [Header("Speed Control")]
    [SerializeField] private float moveMultiplier;
    [SerializeField] private float airMultiplier;
    private Vector3 flatVelocity;
    public float groundDrag;

    public void StateHandler()
    {
        if (_playerActions.climbScript.isClimbing)
        {
            state = PlayerState.wallClimb;
            desiredMoveSpeed = runSpeed;
        }
        else if (_playerActions.wallRunningScript.isWallRunning)
        {
            state = PlayerState.wallRun;
            desiredMoveSpeed = wallRunSpeed;
        }
        else if (_playerActions.slideScript.isSliding)
        {
            state = PlayerState.slide;

            // increase speed by one every second
            if (_playerActions.slideScript.isSlideDown)
            {
                desiredMoveSpeed = slideSpeed;

                float slopeAngle = Vector3.Angle(Vector3.up, _playerActions.managerScript.slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                speedChangeFactor = slopeIncreaseMultiplier * slopeAngleIncrease;
                
                keepMomentum = true;
            }
            else
            {
                desiredMoveSpeed = runSpeed;

                speedChangeFactor = slopeIncreaseMultiplier;

                keepMomentum = true;
            }
            return;
        }
        else if (_playerActions.dashScript.dashed)
        {
            state = PlayerState.dash;
            desiredMoveSpeed = dashSpeed;

            speedChangeFactor = dashIncreaseMultiplier;
        }
        else if (_playerActions.crouchScript.isCrouched)
        {
            state = PlayerState.crouch;

            desiredMoveSpeed = crouchedSpeed;

        }
        else if (_playerActions.managerScript.IsGrounded())
        {
            state = PlayerState.run;

            desiredMoveSpeed = runSpeed;

            speedChangeFactor = speedIncreaseMultiplier;
        }
        else if (!_playerActions.managerScript.IsGrounded())
        {
            state = PlayerState.air;

            if (moveSpeed < airMinSpeed)
            {
                desiredMoveSpeed = airMinSpeed;
            }

            speedChangeFactor = airborneIncreaseMultiplier;

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();

        StateHandler();

        DesiredMoveSpeed();

        SpeedControl();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        LimitVelocity();
    }

    public override void ActionMethod()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moving = (horizontalInput != 0 || verticalInput != 0);
    }

    public void UpdateDirection()
    {
        moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
    }

    public void ApplyMovement()
    {
        if (_playerActions.dashScript.dashed || _playerActions.wallRunningScript.isWallRunning || _playerActions.climbScript.isClimbing)
        {
            return;
        }

        if (_playerActions.managerScript.OnSlope() && !_playerActions.managerScript.exitingSlope)
        {
            slopedDirection = _playerActions.managerScript.GetSlopeDirection(moveDirection);

            _playerActions.rigidBody.AddForce(slopedDirection * moveSpeed * moveMultiplier * 2f, ForceMode.Force);

            
            if (_playerActions.rigidBody.velocity.y > 5)
            {
                _playerActions.rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

            _playerActions.rigidBody.drag = groundDrag;
        }
        else if (_playerActions.managerScript.IsGrounded())
        {
            _playerActions.rigidBody.AddForce(moveDirection * moveSpeed * moveMultiplier, ForceMode.Force);

            _playerActions.rigidBody.drag = groundDrag;
        }
        else if (!_playerActions.managerScript.IsGrounded())
        {
            _playerActions.rigidBody.AddForce(moveDirection * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Force);

            _playerActions.rigidBody.drag = 0;
        } 

        // Live testing
        currentVelocity = new Vector3(_playerActions.rigidBody.velocity.x, 0f, _playerActions.rigidBody.velocity.z).magnitude;

        _playerActions.rigidBody.useGravity = !_playerActions.managerScript.OnSlope();
    }

    public void SpeedControl()
    {
        flatVelocity = new Vector3(_playerActions.rigidBody.velocity.x, 0f, _playerActions.rigidBody.velocity.z);
    }

    public void LimitVelocity()
    {
        if (_playerActions.managerScript.OnSlope() && !_playerActions.managerScript.exitingSlope)
        {
            if (_playerActions.rigidBody.velocity.magnitude > moveSpeed)
            {
                _playerActions.rigidBody.velocity = _playerActions.rigidBody.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                _playerActions.rigidBody.velocity = new Vector3(limitedVelocity.x, _playerActions.rigidBody.velocity.y, limitedVelocity.z);
            }

            if (maxYSpeed != 0 && _playerActions.rigidBody.velocity.y > maxYSpeed)
            {
                _playerActions.rigidBody.velocity = new Vector3(_playerActions.rigidBody.velocity.x, maxYSpeed, _playerActions.rigidBody.velocity.z);
            }
        }
    }

    public void DesiredMoveSpeed()
    {
        if (desiredMoveSpeed != lastDesiredMoveSpeed)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpSpeeds());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;

        // deactivate keepMomentum
        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpSpeeds()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
                
            time += Time.deltaTime * speedChangeFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
}
