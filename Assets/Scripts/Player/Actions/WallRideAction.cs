using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRideAction : AbstractAction
{
    [Header("Wall Running")]
    public bool isWallRunning;
    public bool exitingWall;
    private RaycastHit currentWall;

    [Space(10)]

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Space(10)]

    [Header("Stats")]
    [SerializeField] private float wallRunForce;
    public float currentRisingSpeed;
    public float wallRisingSpeed;
    public float noRisingSpeed;

    //[SerializeField] private float maxWallRunTime;
    //private float wallRunTimer;

    [SerializeField] private float maxCameraTilt;
    [SerializeField] private float cameraTilt;

    [Space(10)]

    [Header("Detections")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minWallRunHeight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool wallLeft, wallRight;

    [Space(10)]

    [Header("References")]
    [SerializeField] private Transform playerCam;
    [SerializeField] private PlayerCamera cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        isWallRunning = false;
        exitingWall = false;
        currentRisingSpeed = wallRisingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        InputController();
    }

    void FixedUpdate()
    {
        if (!exitingWall && isWallRunning)
        {
            WallRunMovement();
        }
        else
        {
            RotateCameraBack();
        }
    }

    public override void ActionMethod()
    {
        StartWallRun();
    }

    public void InputController()
    {
        horizontalInput = _playerActions.movementScript.horizontalInput;
        verticalInput = _playerActions.movementScript.verticalInput;

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            if (!isWallRunning)
            {
                ActionMethod();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    public void StartWallRun()
    {
        isWallRunning = true;


        _playerActions.jumpScript.numOfJumps = 0;


        if (wallRight)
        {
            currentWall = rightWallHit;
        }
        else if (wallLeft)
        {
            currentWall = leftWallHit;
        }
    }

    public void WallRunMovement()
    {
        //wallRunTimer += Time.deltaTime;

        _playerActions.rigidBody.useGravity = false;
        _playerActions.rigidBody.velocity = new Vector3(_playerActions.rigidBody.velocity.x, currentRisingSpeed, _playerActions.rigidBody.velocity.z);

        Vector3 wallNormal = currentWall.normal;

        _playerActions.jumpScript.awayFromWallNormal = wallNormal;

        Vector3 wallForward = Vector3.Cross(wallNormal, currentWall.transform.up);

        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        if (Mathf.Abs(cameraTilt) < maxCameraTilt && wallRight)
        {
            cameraTilt += Time.deltaTime * maxCameraTilt * 4;
        } 
        if (Mathf.Abs(cameraTilt) < maxCameraTilt &&  wallLeft)
        {
            cameraTilt -= Time.deltaTime * maxCameraTilt * 4;
        }

        cameraScript.rotateOtherly = cameraTilt;

        _playerActions.rigidBody.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if ((!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)))
        {
            _playerActions.rigidBody.AddForce(-wallNormal * 100f, ForceMode.Force);
        }
    }

    public void StopWallRun()
    {
        isWallRunning = false;
    }

    public void RotateCameraBack()
    {
        if (cameraTilt != 0)
        {
            if (cameraTilt > 0 && (!wallRight || !wallLeft))
            {
                cameraTilt -= Time.deltaTime * maxCameraTilt * 4;
            }
            if (cameraTilt < 0 && (!wallRight || !wallLeft))
            {
                cameraTilt += Time.deltaTime * maxCameraTilt * 4;
            }

            cameraScript.rotateOtherly = cameraTilt;
        }
    }

    public void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, transform.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallHit, wallCheckDistance, whatIsWall);
        
    }

    public bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minWallRunHeight, whatIsGround);
    }
}
