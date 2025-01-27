using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunItemScriptableObject : ScriptableObject
{
    [Header("Statistics")]
    [Header("Damage")]
    public float damagePerShot;
    public float critChance;
    public float critMultiplier;
    public float armorPiercing;
    [Space(15)]
    [Header("Fire Rate & Accuracy")]
    public float shotsPerSecond;
    public float shotSpread;
    public bool burstFire;
    public float burstResetTime;
    public bool shotgunFire;
    public int bulletsPerBurst;
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
    public string specialEffect;
}
