using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    private HealthBar healthBar;

    [Header("Shield Settings")]
    public float shieldDuration = 5f;         // Duration in seconds
    private float shieldTimeRemaining;
    private bool isShieldActive = false;
    public GameObject forcefield;             // Assign visual effect in Inspector
    private ShieldBar shieldBar;

    [HideInInspector]
    public bool isOnCheckpoint = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Find HealthBar by tag
        GameObject healthBarObj = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarObj != null)
        {
            healthBar = healthBarObj.GetComponent<HealthBar>();
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar GameObject not found with tag 'HealthBar'.");
        }

        // Find ShieldBar by tag
        GameObject shieldBarObj = GameObject.FindGameObjectWithTag("ShieldBar");
        if (shieldBarObj != null)
        {
            shieldBar = shieldBarObj.GetComponent<ShieldBar>();
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(0f); // Keep shield bar visible but empty at start
        }
        else
        {
            Debug.LogWarning("ShieldBar GameObject not found with tag 'ShieldBar'.");
        }

        // Make sure forcefield starts disabled (gets enabled in ActivateShield)
        if (forcefield != null)
            forcefield.SetActive(false);

        // Automatically activate shield on game start
        ActivateShield();
    }

    void Update()
    {
        // Press Y to test damage (optional)
        if (Input.GetKeyDown(KeyCode.Y))
            TakeDamage(5);

        if (isShieldActive && !isOnCheckpoint)
        {
            shieldTimeRemaining -= Time.deltaTime;

            if (shieldBar != null)
                shieldBar.SetTime(shieldTimeRemaining);

            if (shieldTimeRemaining <= 0f)
                DeactivateShield();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            Debug.Log("Damage blocked by shield.");
            return;
        }

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
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
