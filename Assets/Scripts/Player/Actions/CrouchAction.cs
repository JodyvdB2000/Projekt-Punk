using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchAction : AbstractAction
{
    [Header("Crouching")]
    [Tooltip("Crouched speed will be slow in comparison.")]
    [SerializeField] private CapsuleCollider capsuleColliderMain;
    [SerializeField] private float crouchYScaleMain;
    [SerializeField] private float startYScaleMain;
    [SerializeField] private float crouchCenterMain;
    [SerializeField] private float startCenterMain;

    [SerializeField] private CapsuleCollider capsuleColliderSlip;
    [SerializeField] private float crouchYScaleSlip;
    [SerializeField] private float startYScaleSlip;
    [SerializeField] private float crouchCenterSlip;
    [SerializeField] private float startCenterSlip;

    [SerializeField] private Transform cameraHeight;
    [SerializeField] private float crouchCameraHeight;
    [SerializeField] private float startCameraHeight;

    [SerializeField] private Transform headChecker;
    [SerializeField] private float crouchHeadCheckerHeight;
    [SerializeField] private float startHeadCheckerHeight;

    public bool isCrouched;

    // Start is called before the first frame update
    void Start()
    {
        isCrouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMethod()
    {
        //transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

        capsuleColliderMain.height = crouchYScaleMain;
        capsuleColliderMain.center = new Vector3(0f, crouchCenterMain, 0f);

        capsuleColliderSlip.height = crouchYScaleSlip;
        capsuleColliderSlip.center = new Vector3(0f, crouchCenterSlip, 0f);

        headChecker.localPosition = new Vector3(headChecker.localPosition.x, crouchHeadCheckerHeight, headChecker.localPosition.z);
        cameraHeight.localPosition = new Vector3(cameraHeight.localPosition.x, crouchCameraHeight, cameraHeight.localPosition.z);


        isCrouched = true;
    }

    public void StayCrouching()
    {
        isCrouched = true;
    }

    public void CrouchStopMethod()
    {
        StartCoroutine(WaitForTunnelExit()); 
    }

    public IEnumerator WaitForTunnelExit()
    {
        yield return new WaitUntil(() => !_playerActions.managerScript.InTunnel());

        //transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        capsuleColliderMain.height = startYScaleMain;
        capsuleColliderMain.center = new Vector3(0f, startCenterMain, 0f);

        capsuleColliderSlip.height = startYScaleSlip;
        capsuleColliderSlip.center = new Vector3(0f, startCenterSlip, 0f);

        headChecker.localPosition = new Vector3(headChecker.localPosition.x, startHeadCheckerHeight, headChecker.localPosition.z);
        cameraHeight.localPosition = new Vector3(cameraHeight.localPosition.x, startCameraHeight, cameraHeight.localPosition.z);


        isCrouched = false;
    }
}
