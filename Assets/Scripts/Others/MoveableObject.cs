using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyForce(float magnitude, Vector3 forceApplyposition, Vector3 playerPosition)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 forceDirection = -(playerPosition - transform.position);
        Vector3 force = forceDirection.normalized * magnitude;

        rb.AddForceAtPosition(force, forceApplyposition, ForceMode.Impulse);
    }
}
