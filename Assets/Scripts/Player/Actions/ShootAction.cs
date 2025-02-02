using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using EZCameraShake;

public class ShootAction : AbstractAction
{
    [Header("References")]
    [SerializeField] private GunStats gunStatsScript;

    [Header("Active Trackers")]
    [SerializeField] private bool readyToShoot;
    public bool shooting;
    public bool reloading;
    public int currentAmmo;
    private float timeToNextShot;
    private int bulletsLeftInBurst;

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
        gunStatsScript.timeToNextShot = 1 / gunStatsScript.shotsPerSecond;
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

    public void ResetGun()
    {
        currentAmmo = gunStatsScript.maxMagazineSize;
        readyToShoot = true;
        reloading = false;
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

                FireBullet();

                if (gunStatsScript.burstFire)
                {
                    bulletsLeftInBurst = gunStatsScript.bulletsPerBurst - 1;
                    Invoke(nameof(BurstFire), gunStatsScript.burstResetTime);
                }
                else
                {
                    Invoke(nameof(ResetShot), gunStatsScript.timeToNextShot);
                }

                
            }
        }
        else
        {
            Reload();
        }
    }

    public void BurstFire()
    {
        if (currentAmmo > 0)
        {
            if (bulletsLeftInBurst > 0)
            {
                FireBullet();
                bulletsLeftInBurst--;
                Invoke(nameof(BurstFire), gunStatsScript.burstResetTime);
            }
            else if (bulletsLeftInBurst == 0)
            {
                Invoke(nameof(ResetShot), gunStatsScript.timeToNextShot);
            }
        }
    }

    public void FireBullet()
    {
        currentAmmo--;

        // Bullet Spread:
        float xSpread = Random.Range(-gunStatsScript.shotSpread, gunStatsScript.shotSpread);
        float ySpread = Random.Range(-gunStatsScript.shotSpread, gunStatsScript.shotSpread);

        Vector3 newShotDirection = raycastOrigin.forward + new Vector3(xSpread, ySpread, 0);

        Debug.Log(xSpread + ", " + ySpread);

        if (Physics.Raycast(raycastOrigin.position, newShotDirection, out aimHit, raycastLength, layer, triggerInteraction))
        {
            crosshairOnEnemy = true;
            targetCollider = aimHit.collider;
            if (targetCollider.gameObject.tag.Equals("Enemy"))
            {
                targetCollider.GetComponent<EnemyHealth>().TakeDamage(gunStatsScript.damagePerShot);
            }
            else if (targetCollider.gameObject.tag.Equals("Moveable"))
            {
                targetCollider.GetComponent<MoveableObject>().ApplyForce(gunStatsScript.damagePerShot, aimHit.point, transform.position);
            }
        }

        CameraShaker.Instance.ShakeOnce(gunStatsScript.cameraShakeMagnitude, gunStatsScript.cameraShakeRoughness, gunStatsScript.cameraShakeFadeInTime, gunStatsScript.cameraShakeFadeOutTime);
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
