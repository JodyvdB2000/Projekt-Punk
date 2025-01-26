using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWallRunHeight : MonoBehaviour
{
    private WallRideAction wallRidingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<WallRideAction>() != null)
        {
            wallRidingPlayer = collider.gameObject.GetComponent<WallRideAction>();
            wallRidingPlayer.currentRisingSpeed = wallRidingPlayer.noRisingSpeed;

        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<WallRideAction>() != null)
        {
            wallRidingPlayer = collider.gameObject.GetComponent<WallRideAction>();
            wallRidingPlayer.currentRisingSpeed = wallRidingPlayer.noRisingSpeed;

        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<WallRideAction>() != null)
        {
            wallRidingPlayer = collider.gameObject.GetComponent<WallRideAction>();
            wallRidingPlayer.currentRisingSpeed = wallRidingPlayer.wallRisingSpeed;
        }
    }
}
