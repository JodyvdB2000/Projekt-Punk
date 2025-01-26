using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Slider healthBar;

    private Camera playerCam;

    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
        enemyCanvas.worldCamera = playerCam;

        SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCanvas.transform.LookAt(playerCam.transform);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        healthBar.value = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;
        SetHealthBar();
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
