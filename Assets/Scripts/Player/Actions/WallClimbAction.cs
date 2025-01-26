using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbAction : AbstractAction
{
    [Header("Wall Climbing")]
    public bool isClimbing;
    public bool exitingWall;
    private float verticalInput;

    [Space(10)]
    
    [Header("Stats")]
    [SerializeField] private float climbSpeed;
    [SerializeField] private float maxClimbTime;
    private float climbTimer;
    [SerializeField] private float exitWallTime;

    [Space(10)]

    [Header("References")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit wallFrontHit;
    private bool wallFront;

    // Start is called before the first frame update
    void Start()
    {
        isClimbing = false;
        exitingWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        InputController();
    }

    void FixedUpdate()
    {
        if (!exitingWall && isClimbing)
        {
            ClimbMovement();
        }
    }

    public void InputController()
    {
        verticalInput = _playerActions.movementScript.verticalInput;

        if (wallFront && verticalInput > 0 && wallLookAngle <= maxWallLookAngle && climbTimer < maxClimbTime)
        {
            if (!isClimbing)
            {
                ActionMethod();
            }
        }
        else
        {
            StopWallClimb();
        }
    }

    public override void ActionMethod()
    {
        StartWallClimb();
    }

    public void CheckForWall()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out wallFrontHit, wallCheckDistance, whatIsWall);
        
        if (wallFront) 
        {
            wallLookAngle = Vector3.Angle(transform.forward, -wallFrontHit.normal);
            _playerActions.jumpScript.awayFromWallNormal = wallFrontHit.normal;
        }
    }

    public void StartWallClimb()
    {
        isClimbing = true;
        climbTimer = 0f;
        _playerActions.jumpScript.numOfJumps = 0;
    }

    public void ClimbMovement()
    {
        climbTimer += Time.deltaTime;

        _playerActions.rigidBody.velocity = new Vector3(0f, climbSpeed, 0f);
    }

    public void StopWallClimb()
    {
        bool checkedExit = false;
        if (exitingWall && !checkedExit)
        {
            Invoke(nameof(TurnOffClimb), exitWallTime);
            checkedExit = true;
        }
        else
        {
            TurnOffClimb();
            checkedExit = true;
        }
    }

    public void TurnOffClimb()
    {
        isClimbing = false;
        exitingWall = false;
    }

    public void ResetClimb()
    {
        Invoke(nameof(AllowClimb), 0.75f);
    }

    public void AllowClimb()
    {
        climbTimer = 0f;
    }
}
