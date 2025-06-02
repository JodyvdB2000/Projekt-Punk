using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    [Header("Statistics")]
    [Header("Camera")]
    public float cameraShakeRoughness;
    public float cameraShakeMagnitude; 
    public float cameraShakeFadeInTime;
    public float cameraShakeFadeOutTime;
    [Space(15)]
    [Header("Damage")]
    public float damagePerShot;
    public float critChance;
    public float critMultiplier;
    public float armorPiercing;
    [Space(15)]
    [Header("Fire Rate & Accuracy")]
    public float shotsPerSecond;
    public float timeToNextShot;
    public float shotSpread;
    public bool burstFire;
    public int bulletsPerBurst;
    public float burstResetTime;
    public bool shotgunFire;
    public int bulletsPerShotgun;
    [Space(15)]
    [Header("Magazine & Ammo Types")]
    public int maxMagazineSize;
    public float reloadSpeed;
    public bool replaceWithProjectile;
    public GameObject projectileObject;
    [Space(15)]
    [Header("Scope & Range")]
    public bool scopeActive;
    public float scopeInAperture;
    public float damageRangeDropoff;
    [Space(15)]
    [Header("Status Effects")]
    public StatusEffects.statusEffectsEnum statusEffect;
    public float statusChance;
    [Space(15)]
    [Header("Laser")]
    public bool laserActive;

    [Space(15)]
    [Header("SPECIAL")]
    public List<string> specialEffects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewStats(GunItemScriptableObject newItem)
    {
        damagePerShot += newItem.damagePerShot;
        critChance += newItem.critChance;
        critMultiplier += newItem.critMultiplier;

        shotsPerSecond += newItem.shotsPerSecond;
        timeToNextShot = 1 / shotsPerSecond;

        shotSpread += newItem.shotSpread;

        if (newItem.applyBurst)
        {
            burstFire = newItem.burstFire;
            if (burstFire)
            {
                bulletsPerBurst += newItem.bulletsPerBurst;
                burstResetTime += newItem.burstResetTime;
            }
        }

        if (newItem.applyShotgun)
        {
            shotgunFire = newItem.shotgunFire;
            if (shotgunFire)
            {
                bulletsPerShotgun += newItem.bulletsPerShotgun;
            }
        }

        maxMagazineSize += newItem.maxMagazineSize;
        reloadSpeed += newItem.reloadSpeed;

        if (newItem.applyProjectile)
        {
            replaceWithProjectile = newItem.replaceWithProjectile;
            if (replaceWithProjectile)
            {
                projectileObject = newItem.projectileObject;
            }
        }

        if (newItem.applyScope)
        {
            scopeActive = newItem.scopeActive;
            scopeInAperture += newItem.scopeInAperture;
        }

        damageRangeDropoff += newItem.damageRangeDropoff;

        if (newItem.applyStatus)
        {
            statusEffect = newItem.statusEffect;
            if (statusEffect != StatusEffects.statusEffectsEnum.None)
            {
                statusChance += newItem.statusChance;
            }
        }

        if (newItem.applyLaser)
        {
            laserActive = newItem.laserActive;
        }

        specialEffects.Add(newItem.specialEffect);
    }
}
