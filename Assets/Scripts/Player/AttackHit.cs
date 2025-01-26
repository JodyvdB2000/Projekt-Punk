using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int bulletDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<EnemyHealth>() != null)
        {
            EnemyHealth targetHealth = collider.gameObject.GetComponent<EnemyHealth>();

            targetHealth.TakeDamage(bulletDamage);

        }
    }
}
