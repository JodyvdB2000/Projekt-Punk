using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    [SerializeField] private ShootAction currentGunScript;
    [SerializeField] private GunStats currentGunStats;

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
        if (collider.gameObject.tag.Equals("GunItem"))
        {
            Debug.Log("Picked Up" + collider.gameObject.name);
            GunItemScriptableObject newItem = collider.gameObject.GetComponent<GunItemPickup>().GunItemStats;

            currentGunStats.AddNewStats(newItem);

            Destroy(collider.gameObject);
        }

        currentGunScript.ResetGun();
    }
}
