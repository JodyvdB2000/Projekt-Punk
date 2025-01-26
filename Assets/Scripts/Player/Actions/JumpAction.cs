using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : AbstractAction
{
    [Header("Jumping")]
    [Tooltip("Jumping force")]
    [SerializeField] private float defaultJumpForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private bool readyToJump;
    public int numOfJumps;
    [SerializeField] private int maxJumps;

    [Space(10)]

    [Header("Wall Jumps")]
    public float wallJumpUpForce;
    public float awayFromWallClimbForce;
    public float awayFromWallRunForce;
    [HideInInspector] public Vector3 awayFromWallNormal;

    [Space(10)]

    [Header("Boost Pad")]
    [SerializeField] private float superJumpForce;

    // Start is called before the first frame update
    void Start()
    {
        readyToJump = true;
        jumpForce = defaultJumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        BoostPadJump();
    }

    public override void ActionMethod()
    {
        if (!_playerActions.managerScript.IsGrounded() && numOfJumps >= maxJumps)
        {
            return;
        }

        if (_playerActions.climbScript.isClimbing)
        {
            WallJump(awayFromWallClimbForce);
        }
        else if (_playerActions.wallRunningScript.isWallRunning)
        {
            WallJump(awayFromWallRunForce);
        }
        else
        {
            Jump();
        }

    }

    public void Jump()
    {
        if (readyToJump)
        {
            _playerActions.managerScript.exitingSlope = true;

            _playerActions.rigidBody.velocity = new Vector3(_playerActions.rigidBody.velocity.x, 0f, _playerActions.rigidBody.velocity.z);

            _playerActions.rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            AddJump();

            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void WallJump(float jumpAwayForce)
    {
        if (readyToJump)
        {
            _playerActions.wallRunningScript.exitingWall = true;
            _playerActions.climbScript.exitingWall = true;

            _playerActions.rigidBody.velocity = new Vector3(_playerActions.rigidBody.velocity.x, 0f, _playerActions.rigidBody.velocity.z);

            Vector3 forceToApply = (transform.up * wallJumpUpForce) + (awayFromWallNormal * jumpAwayForce);

            _playerActions.rigidBody.AddForce(forceToApply, ForceMode.Impulse);

            AddJump();

            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void ResetJump()
    {
        readyToJump = true;
        _playerActions.managerScript.exitingSlope = false;

        _playerActions.wallRunningScript.exitingWall = false;
    }

    public void AddJump()
    {
        numOfJumps++;
    }

    public int GetJumps()
    {
        return numOfJumps;
    } 

    public void BoostPadJump()
    {
        if (_playerActions.managerScript.IsOnBooster())
        {
            jumpForce = superJumpForce;
        }
        else
        {
            jumpForce = defaultJumpForce;
        }
    }
}
