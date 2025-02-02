using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunItemScriptableObject : ScriptableObject
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
    public float shotSpread;
    public bool applyBurst;
    public bool burstFire;
    public int bulletsPerBurst;
    public float burstResetTime;
    public bool applyShotgun;
    public bool shotgunFire;
    public int bulletsPerShotgun;
    [Space(15)]
    [Header("Magazine & Ammo Types")]
    public int maxMagazineSize;
    public float reloadSpeed;
    public bool applyProjectile;
    public bool replaceWithProjectile;
    public GameObject projectileObject;
    [Space(15)]
    [Header("Scope & Range")]
    public bool applyScope;
    public bool scopeActive;
    public float scopeInAperture;
    public float damageRangeDropoff;
    [Space(15)]
    [Header("Status Effects")]
    public bool applyStatus;
    public StatusEffects.statusEffectsEnum statusEffect;
    public float statusChance;
    [Space(15)]
    [Header("Laser")]
    public bool applyLaser;
    public bool laserActive;

    [Space(15)]
    [Header("SPECIAL")]
    public string specialEffect;
}
