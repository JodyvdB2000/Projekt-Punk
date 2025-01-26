using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    public enum specialStatus
    {
        None,
        Dissolve,
        Necrotic,
        Limbo,
        Holy,
    }

    [Header("Statistics")]
    [Header("Damage")]
    public float damagePerShot;
    public float critChance;
    public float critMultiplier;
    [Space(15)]
    [Header("Fire Rate & Accuracy")]
    public int bulletsPerShot;
    public float timeBetweenShots;
    public float bulletSpread;
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
    public specialStatus status;
    public float statusChance;
    [Space(15)]
    [Header("Laser")]
    public bool laserActive;
    public float laserAccuracy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
