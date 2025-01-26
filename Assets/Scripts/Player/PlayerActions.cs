using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActions : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Rigidbody rigidBody;

    [HideInInspector] public PlayerManager managerScript;
    [HideInInspector] public UIManager uiScript;

    [HideInInspector] public WallRideAction wallRunningScript;
    [HideInInspector] public WallClimbAction climbScript;
    [HideInInspector] public SlideAction slideScript;
    [HideInInspector] public JumpAction jumpScript;
    [HideInInspector] public DashAction dashScript;
    [HideInInspector] public CrouchAction crouchScript;
    [HideInInspector] public MovementScript movementScript;
    [HideInInspector] public ShootAction shootScript;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        managerScript = GetComponent<PlayerManager>();
        uiScript = GetComponent<UIManager>();

        movementScript = GetComponent<MovementScript>();
        jumpScript = GetComponent<JumpAction>();
        dashScript = GetComponent<DashAction>();
        slideScript = GetComponent<SlideAction>();
        crouchScript = GetComponent<CrouchAction>();
        shootScript = GetComponent<ShootAction>();
        wallRunningScript = GetComponent<WallRideAction>();
        climbScript = GetComponent<WallClimbAction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootAction()
    {
        shootScript.ActionMethod();
    }

    public void ReloadAction()
    {
        shootScript.Reload();
    }

    public void StopShootAction()
    {
        shootScript.StopShooting();
    }

    public void RunAction()
    {
        movementScript.ActionMethod();
    }

    public void JumpAction()
    {
        jumpScript.ActionMethod();
    }

    public void DashAction()
    {
        dashScript.ActionMethod();
    }

    public void CrouchAction()
    {
        crouchScript.ActionMethod();
    }

    public void CrouchStop()
    {
        crouchScript.CrouchStopMethod();
    }

    public void SlideAction()
    {
        slideScript.ActionMethod();
    }

    public void SlideStop()
    {
        slideScript.SlideStopMethod();
    }
}
