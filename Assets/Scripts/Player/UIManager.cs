using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerActions actionsScript;

    [Header("UI Elements")]
    [SerializeField] private Text txtTimer;
    private float timeCount = 0;

    [Space(10)]

    [SerializeField] private Text txtSpeed;
    [SerializeField] private Text txtJumps;
    [SerializeField] private Text txtSliding;
    [SerializeField] private Text txtCrouch;
    [SerializeField] private Text txtDash;
    [SerializeField] private Text txtAimingOnEnemy;
    [SerializeField] private Text txtShoot;
    [SerializeField] private Text txtAmmo;
    [SerializeField] private Text txtWallRun;
    [SerializeField] private Text txtWallClimb;

    private const string CONST_TIMER = "Time - ";
    private const string CONST_SPEED = "Speed [";
    private const string CONST_JUMPS = "Jumps Used [";
    private const string CONST_SLIDING = "Sliding [";
    private const string CONST_CROUCHING = "Crouching [";
    private const string CONST_DASH = "Dashed [";
    private const string CONST_AIMINGONENEMY = "On Enemy [";
    private const string CONST_SHOOT = "Shooting [";
    private const string CONST_RELOAD = "Reloading [";
    private const string CONST_AMMO = "[";
    private const string CONST_WALL_RUN = "Wall Running [";
    private const string CONST_WALL_CLIMB = "Wall Climbing [";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Update
        timeCount += Time.deltaTime;
        txtTimer.text = CONST_TIMER + (int)timeCount;

        // Current Speed Update
        txtSpeed.text = CONST_SPEED + (int)actionsScript.movementScript.currentVelocity + "]";

        txtJumps.text = CONST_JUMPS + actionsScript.jumpScript.GetJumps() + "]";

        if (actionsScript.shootScript.reloading)
        {
            txtShoot.text = CONST_RELOAD + "X]";
        }
        else if (actionsScript.shootScript.shooting)
        {
            txtShoot.text = CONST_SHOOT + "X]";  
        }
        else
        {
            txtShoot.text = CONST_SHOOT + " ]";
        }

        txtAmmo.text = CONST_AMMO + actionsScript.shootScript.currentAmmo + "]";

        if (actionsScript.shootScript.crosshairOnEnemy)
        {
            txtAimingOnEnemy.text = CONST_AIMINGONENEMY + "X]";
        }
        else
        {
            txtAimingOnEnemy.text = CONST_AIMINGONENEMY + " ]";
        }

        if (actionsScript.climbScript.isClimbing)
        {
            txtWallClimb.text = CONST_WALL_CLIMB + "X]";
        }
        else
        {
            txtWallClimb.text = CONST_WALL_CLIMB + " ]";
        }

        if (actionsScript.wallRunningScript.isWallRunning)
        {
            txtWallRun.text = CONST_WALL_RUN + "X]";
        }
        else
        {
            txtWallRun.text = CONST_WALL_RUN + " ]";
        }

        if (actionsScript.slideScript.isSliding)
        {
            txtSliding.text = CONST_SLIDING + "X]";
        }
        else
        {
            txtSliding.text = CONST_SLIDING + " ]";
        }

        if (actionsScript.crouchScript.isCrouched)
        {
            txtCrouch.text = CONST_CROUCHING + "X]";
        }
        else
        {
            txtCrouch.text = CONST_CROUCHING + " ]";
        }

        if (actionsScript.dashScript.dashed)
        {
            txtDash.text = CONST_DASH + "X]";
        }
        else
        {
            txtDash.text = CONST_DASH + " ]";
        }
    }
}
