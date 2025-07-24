using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int _currentHealth;
    private HealthBar healthBar;
    [Header("Shield Settings")]
    public float shieldDuration = 5f;
    private float shieldTimeRemaining;
    private bool isShieldActive = false;
    public GameObject forcefield;
    private ShieldBar shieldBar;

    [Header("Damage Settings")]
    public int damagePerTick = 5;
    public float damageInterval = 1.0f;
    private float damageTimer = 0f;

    // ← add this so Checkpoint.cs can compile
    [HideInInspector]
    public bool isOnCheckpoint = false;

    private GameOverScene gameOverScene;
    private bool isDead = false;

    void Start()
    {
        _currentHealth = maxHealth;
        // … your healthBar/shieldBar setup …

        // load manager (use FindObjectOfType if needed)
        gameOverScene = FindFirstObjectByType<GameOverScene>();
        if (gameOverScene == null)
            Debug.LogWarning("No GameOverScene component found in scene.");
    }

    void Update()
    {
        if (isDead) return;
        // … your shield countdown & auto‐damage …
    }

    public bool TakeDamage(int damage)
    {
        if (isDead) 
            return false;

            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            healthBar?.SetHealth(_currentHealth);
            Debug.Log($"Player took {damage} dmg, health now {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }
    private void Die()
    {
        isDead = true;
        gameOverScene?.ShowGameOver();
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        shieldTimeRemaining = shieldDuration;
        forcefield?.SetActive(true);
        shieldBar?.SetMaxTime(shieldDuration);
        shieldBar?.SetTime(shieldDuration);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        forcefield?.SetActive(false);
        shieldBar?.SetTime(0f);
    }

}
