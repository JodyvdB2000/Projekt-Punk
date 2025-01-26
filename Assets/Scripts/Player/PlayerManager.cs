using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rigidBody;
    private PlayerController playerController;

    [Header("Grounded")]
    [SerializeField] private Transform feet;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;

    private bool isWaitForLandingRunning;
    private bool falling;
    private bool rising;

    [Space(10)]

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private bool onSlope;
    public bool exitingSlope;
    public RaycastHit slopeHit;

    [Space(10)]

    [Header("In Tunnel")]
    [SerializeField] private Transform head;
    public RaycastHit tunnelHit;
    [SerializeField] private bool inTunnel;

    [Space(10)]

    [Header("Boost Pad")]
    [SerializeField] private LayerMask whatIsBooster;
    [SerializeField] private bool isOnBooster;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        IsGrounded();

        InTunnel();

        IsFalling();
        IsRising();
    }

    public bool IsGrounded()
    {
        if (Physics.CheckSphere(feet.position, 0.1f, whatIsGround))
        {
            isWaitForLandingRunning = false;

            isGrounded = true;

            OnSlope();

            return isGrounded;
        }


        if (!isWaitForLandingRunning)
        {
            StartCoroutine(WaitForLanding());
        }

        onSlope = false;
        isGrounded = false;

        return isGrounded;
    }

    public bool IsFalling()
    {
        if (!isGrounded && !onSlope && rigidBody.velocity.y < -2f)
        {
            falling = true;
            return falling;
        }
        falling = false;
        return falling;
    }

    public bool IsRising()
    {
        if (!isGrounded && !onSlope && rigidBody.velocity.y > 1f)
        {
            rising = true;
            return rising;
        }
        rising = false;
        return rising;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle < maxSlopeAngle && angle != 0f)
            {
                onSlope = true;
                return onSlope;
            }
        }

        onSlope = false;
        return onSlope;
    }

    public Vector3 GetSlopeDirection(Vector3 normalDirection)
    {
        Vector3 dir = Vector3.ProjectOnPlane(normalDirection, slopeHit.normal).normalized;

        return dir;
    }

    public bool InTunnel()
    {
        if (Physics.BoxCast(head.position, new Vector3(0.4f, 0.05f, 0.4f), Vector3.up))
        {
            inTunnel = true;
            return inTunnel;
        }
        inTunnel = false;
        return inTunnel;
    }

    public bool IsOnBooster()
    {
        if (Physics.CheckSphere(feet.position, 0.1f, whatIsBooster))
        {
            isOnBooster = true;

            return isOnBooster;
        }

        isOnBooster = false;

        return isOnBooster;
    }

    private IEnumerator WaitForLanding()
    {
        isWaitForLandingRunning = true;

        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        playerController.actionsScript.jumpScript.numOfJumps = 0;
        playerController.actionsScript.climbScript.ResetClimb();
        isWaitForLandingRunning = false;

        if (Input.GetKey(playerController.inputObject.scheme.GetKeyCode("Crouch")))
        {
            if (playerController.actionsScript.movementScript.moving)
            {
                playerController.actionsScript.SlideAction();
            }
            else
            {
                playerController.actionsScript.CrouchAction();
            }
        }
    }
}
