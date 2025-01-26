using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashAction : AbstractAction
{
    [Header("Dashing")]
    [Tooltip("Dashing force.")]
    public float dashForce;
    private Vector3 forceToApply;
    public float dashUpwardForce;
    public float maxDashYSpeed;

    [Space(10)]

    [SerializeField] private float dashDuration;
    [SerializeField] private AnimationCurve dashCurve;

    [Space(10)]

    public bool dashed;

    [Space(10)]

    [SerializeField] private float dashCooldown;
    private float dashCdTimer;
    [SerializeField] protected bool readyToDash;

    [Space(10)]

    [Header("Settings")]
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Space(10)]

    [Header("Camera Effects")]
    [SerializeField] private bool useCameraForward;
    public Transform playerCam;
    [SerializeField] protected PlayerCamera playerCamScript;
    public float dashFov;

    // Start is called before the first frame update
    void Start()
    {
        if (playerCam == null)
        {
            playerCam = Camera.main.transform;
        }      
    }

    // Update is called once per frame
    void Update()
    {
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    public override void ActionMethod()
    {
        Vector3 direction;
        if (_playerActions.movementScript.moving)
        {
            direction = _playerActions.movementScript.moveDirection;
        }
        else
        {
            direction = transform.forward;
        }

        Dash(useCameraForward);
    }

    public void Dash(bool dashToCam)
    {
        // cooldown implementation
        if (!readyToDash)
        {
            return;
        }

        playerCamScript.DoFov(dashFov);

        // this will cause the PlayerMovement script to change to MovementMode.dashing
        dashed = true;
        _playerActions.movementScript.maxYSpeed = maxDashYSpeed;

        // call the GetDirection() function below to calculate the direction
        Vector3 direction;

        // decide wheter you want to use the playerCam or the playersOrientation as forward direction
        if (dashToCam)
        {
            direction = playerCam.forward; // where you're looking
        }
        else if (_playerActions.movementScript.moving) 
        {
            direction = _playerActions.movementScript.moveDirection; // where you're facing (no up or down)
        }
        else
        {
            direction = transform.forward;
        }

        // calculate the forward and upward force
        Vector3 directionalForceToApply = direction * dashForce + Vector3.up * dashUpwardForce;

        // disable gravity of the players rigidbody if needed
        if (disableGravity)
        {
            _playerActions.rigidBody.useGravity = false;
        }

        _playerActions.rigidBody.drag = 0;

        // add the dash force (deayed)
        forceToApply = directionalForceToApply;

        // limit y speed
        if (forceToApply.y > maxDashYSpeed)
        {
            forceToApply = new Vector3(forceToApply.x, maxDashYSpeed, forceToApply.z);
        }

        readyToDash = false;

        Invoke(nameof(DelayedDashForce), 0.025f);

        // make sure the dash stops after the dashDuration is over
        Invoke(nameof(StopDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        if (resetVel)
        {
            _playerActions.rigidBody.velocity = Vector3.zero;
        }

        _playerActions.rigidBody.AddForce(forceToApply, ForceMode.Impulse);

        
    }

    public void StopDash()
    {
        dashed = false;
        _playerActions.movementScript.maxYSpeed = 0;

        playerCamScript.DoFov(playerCamScript.defaultFov);

        _playerActions.rigidBody.useGravity = true;
        _playerActions.rigidBody.drag = _playerActions.movementScript.groundDrag;

        Invoke(nameof(ResetDash), dashCooldown);
    }

    public void DashToPos(Vector3 startPos, Vector3 targetPos, float elapsedTime, float desiredDashLength)
    {
        float percentageComplete = elapsedTime / desiredDashLength;

        _playerActions.rigidBody.MovePosition(Vector3.Lerp(startPos, targetPos, dashCurve.Evaluate(percentageComplete)));
    }

    public void ResetDash()
    {
        readyToDash = true;
    }
}
