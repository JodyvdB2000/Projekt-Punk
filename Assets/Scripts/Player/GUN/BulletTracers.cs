using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracers : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask mask;
    private float lastShootTime;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireParticle(RaycastHit hit)
    {
        TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, hit));

        lastShootTime = Time.time;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;

        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        Destroy(trail.gameObject, trail.time);
    }
}
