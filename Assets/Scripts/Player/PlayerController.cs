using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    #region Input
    [Header("References")]
    [HideInInspector] public PlayerActions actionsScript;
    [HideInInspector] public PlayerManager managerScript;

    [Header("Input Keys")]
    public ControlSchemeScriptableObject inputObject;

    private const string JUMP_STRING = "Jump";
    private const string DASH_STRING = "Dash";
    private const string CROUCH_STRING = "Crouch";
    private const string SHOOT_STRING = "Shoot";
    private const string RELOAD_STRING = "Reload";

    #endregion

    // Awake is called before Start
    void Awake()
    {
        actionsScript = GetComponent<PlayerActions>();
        managerScript = GetComponent<PlayerManager>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ActionHandler();
    }
    
    public void ActionHandler()
    {
        actionsScript.RunAction();

        if (Input.GetKeyDown(inputObject.scheme.GetKeyCode(JUMP_STRING)))
        {
            actionsScript.JumpAction();
        }

        if (Input.GetKey(inputObject.scheme.GetKeyCode(SHOOT_STRING)))
        {
            actionsScript.ShootAction();
        }
        else if (!Input.GetKey(inputObject.scheme.GetKeyCode(SHOOT_STRING)))
        {
            actionsScript.StopShootAction();
        }

        if (Input.GetKeyDown(inputObject.scheme.GetKeyCode(RELOAD_STRING)))
        {
            actionsScript.ReloadAction();
        }

        if (Input.GetKeyDown(inputObject.scheme.GetKeyCode(DASH_STRING)))
        {
            actionsScript.DashAction();
        }

        if (actionsScript.movementScript.moving && managerScript.IsGrounded() && Input.GetKeyDown(inputObject.scheme.GetKeyCode(CROUCH_STRING)))
        {
            actionsScript.SlideAction();
        }
        else if (!actionsScript.movementScript.moving && managerScript.IsGrounded() && Input.GetKeyDown(inputObject.scheme.GetKeyCode(CROUCH_STRING)))
        {
            actionsScript.CrouchAction();
        }

        if (Input.GetKeyUp(inputObject.scheme.GetKeyCode(CROUCH_STRING)))
        {
            actionsScript.CrouchStop();
            actionsScript.SlideStop();
        }
    }
}
