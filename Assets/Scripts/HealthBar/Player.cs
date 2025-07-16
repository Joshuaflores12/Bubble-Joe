using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // Find the HealthBar in the scene using a tag
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(5);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}
