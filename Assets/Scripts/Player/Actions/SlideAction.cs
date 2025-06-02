using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideAction : AbstractAction
{
    [Header("Sliding")]
    [Tooltip("Speed at which to start the slide action with.")]
    [SerializeField] private float slideForce;

    [Space(5)]
    public bool isSliding;
    public bool isSlideUp;
    public bool isSlideDown;

    [Space(10)]

    [SerializeField] private float maxSlideTime;
    [SerializeField] private float slideTimer;

    [Space(10)]

    [SerializeField] private CrouchAction crouchActionScript;

    private Vector3 slopedDirection;

    [SerializeField] protected PlayerCamera playerCamScript;
    [SerializeField] private float slideFov;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            SlideMovement();
        }
    }

    public override void ActionMethod()
    {
        if (!_playerActions.crouchScript.isCrouched)
        {
            isSliding = true;

            playerCamScript.DoFov(slideFov);

            crouchActionScript.ActionMethod();

            _playerActions.rigidBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            slideTimer = maxSlideTime;
        }
    }

    public void SlideMovement()
    {
        float reductionRate = Time.deltaTime;
        if (_playerActions.managerScript.OnSlope() && _playerActions.rigidBody.velocity.y > -0.1f)
        {
            isSlideDown = false;
            isSlideUp = true;
            reductionRate = Time.deltaTime * 2f;
        }
        if (!_playerActions.managerScript.OnSlope() || _playerActions.rigidBody.velocity.y > -0.1f)
        {
            _playerActions.rigidBody.AddForce(_playerActions.movementScript.moveDirection * slideForce, ForceMode.Force);
            
            isSlideUp = false;
            isSlideDown = false;
        }
        else
        {
            slopedDirection = _playerActions.managerScript.GetSlopeDirection(_playerActions.movementScript.moveDirection);

            _playerActions.rigidBody.AddForce(slopedDirection * slideForce, ForceMode.Force);

            isSlideUp = false;
            isSlideDown = true;
        }

        if (!isSlideDown && _playerActions.managerScript.IsGrounded())
        {
            slideTimer -= reductionRate;
        }

        if (slideTimer <= 0)
        {
            SlideStopMethod();
            _playerActions.crouchScript.StayCrouching();
        }
    }

    public void SlideStopMethod()
    {
        playerCamScript.DoFov(playerCamScript.defaultFov);

        isSliding = false;
        isSlideUp = false;
        isSlideDown = false;
    }
}
