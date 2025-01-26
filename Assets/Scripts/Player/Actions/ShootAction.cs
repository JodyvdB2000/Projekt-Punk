using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class ShootAction : AbstractAction
{
    [Header("References")]
    [SerializeField] private GunStats gunStatsScript;

    [Header("Active Trackers")]
    [SerializeField] private bool readyToShoot;
    public bool shooting;
    public bool reloading;
    public int currentAmmo;

    [Space(15)]

    [Header("Checks")]
    public bool crosshairOnEnemy;

    [Space(10)]
    
    [Header("Raycasting")]
    public Transform raycastOrigin;
    public QueryTriggerInteraction triggerInteraction;
    public LayerMask layer;
    public float raycastLength;

    private RaycastHit aimHit;

    [Header("Target")]
    [SerializeField] private Collider targetCollider;

    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        shooting = false;

        currentAmmo = gunStatsScript.maxMagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastCrosshair();
        if (shooting)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void ForceApplication()
    {
        
    }

    public override void ActionMethod()
    {
        if (!readyToShoot || reloading || currentAmmo <= 0)
        {
            return;
        }

        shooting = true;
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (readyToShoot && !reloading)
            {
                readyToShoot = false;
                currentAmmo--;

                if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out aimHit, raycastLength, layer, triggerInteraction))
                {
                    crosshairOnEnemy = true;
                    targetCollider = aimHit.collider;
                    targetCollider.GetComponent<EnemyHealth>().TakeDamage(gunStatsScript.damagePerShot);
                }

                Invoke(nameof(ResetShot), gunStatsScript.timeBetweenShots);
            }
        }
        else
        {
            Reload();
        }
    }

    public void ResetShot()
    {
        if (currentAmmo > 0)
        {
            readyToShoot = true;
        }
    }

    public void Reload()
    {
        if (!reloading && currentAmmo < gunStatsScript.maxMagazineSize)
        {
            reloading = true;
            readyToShoot = false;

            Invoke(nameof(ResetAfterReload), gunStatsScript.reloadSpeed);
        }
    }

    public void ResetAfterReload()
    {
        currentAmmo = gunStatsScript.maxMagazineSize;
        readyToShoot = true;
        reloading = false;
    }

    public void StopShooting()
    {
        shooting = false;
    }

    public bool RaycastCrosshair()
    {
        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward, Color.green);
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out aimHit, raycastLength, layer, triggerInteraction))
        {
            crosshairOnEnemy = true;
            return crosshairOnEnemy;
        }

        crosshairOnEnemy = false;
        return crosshairOnEnemy;
    }
}
