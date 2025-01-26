using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchAction : AbstractAction
{
    [Header("Crouching")]
    [Tooltip("Crouched speed will be slow in comparison.")]
    [SerializeField] private float crouchYScale;
    [SerializeField] private float startYScale;

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
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

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

        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        isCrouched = false;
    }
}
