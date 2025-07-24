using Unity.Cinemachine;
using UnityEngine;



public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    private HealthBar healthBar;
    
 

    [Header("Shield Settings")]
    public float shieldDuration = 5f;
    private float shieldTimeRemaining;
    private bool isShieldActive = false;
    public GameObject forcefield;
    private ShieldBar shieldBar;

    [Header("Damage Settings")]
    public int damagePerTick = 5;              // Auto damage value
    public float damageInterval = 1.0f;        // How often to apply damage (seconds)
    private float damageTimer = 0f;

    [HideInInspector]
    public bool isOnCheckpoint = false;

    void Start()
    {
        currentHealth = maxHealth;

        GameObject healthBarObj = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarObj != null)
        {
            healthBar = healthBarObj.GetComponent<HealthBar>();
            healthBar.SetMaxHealth(maxHealth);
        }

        GameObject shieldBarObj = GameObject.FindGameObjectWithTag("ShieldBar");
        if (shieldBarObj != null)
        {
            shieldBar = shieldBarObj.GetComponent<ShieldBar>();
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(0f);
        }

        if (forcefield != null)
            forcefield.SetActive(false);

        ActivateShield();
    }

    void Update()
    {
        if (isShieldActive && !isOnCheckpoint)
        {
            shieldTimeRemaining -= Time.deltaTime;

            if (shieldTimeRemaining < 0f)
                shieldTimeRemaining = 0f;

            if (shieldBar != null)
                shieldBar.SetTime(shieldTimeRemaining);

            if (shieldTimeRemaining <= 0f)
                DeactivateShield();
        }

        // Auto damage when shield is empty
        if (shieldTimeRemaining <= 0f && !isOnCheckpoint)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                TakeDamage(damagePerTick);
                damageTimer = 0f;
            }
        }
        else
        {
            damageTimer = 0f; // Reset timer if shield is active
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        Debug.Log("Player took damage: " + damage);
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        shieldTimeRemaining = shieldDuration;

        if (forcefield != null)
            forcefield.SetActive(true);

        if (shieldBar != null)
        {
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(shieldDuration);
        }

        Debug.Log("Shield activated!");
    }

    private void DeactivateShield()
    {
        isShieldActive = false;

        if (forcefield != null)
            forcefield.SetActive(false);

        if (shieldBar != null)
            shieldBar.SetTime(0f);

        Debug.Log("Shield deactivated!");
    }
}
