// PlayerColl.cs
using UnityEngine;
using TMPro;

public class PlayerColl : MonoBehaviour
{

    [Tooltip("How hard the player is pushed back when hit by an enemy")]
    [SerializeField] private float knockbackStrength = 5f;

    [Header("Shield Settings")]
    public float shieldDuration = 5f;
    private float shieldTimeRemaining;
    private bool isShieldActive = false;
    public GameObject forcefield;

    [Header("Shield UI")]
    private ShieldBar shieldBar;

    [Header("Damage Settings")]
    public int damagePerTick = 5;
    public float damageInterval = 1.0f;
    private float damageTimer = 0f;

    [Header("Checkpoint State")]
    [HideInInspector] public bool isOnCheckpoint = false;

    [Header("Shield Cooldown Timer")]
    [Tooltip("How long the countdown lasts when the shield is gone (in seconds).")]
    [SerializeField] private float countdownDuration = 120f; // 2 minutes
    private float countdownRemaining;
    private bool isCountdownActive = false;
    private bool hasFlashed = false;

    [Header("UI")]
    private TMP_Text countdownText;
    private CanvasGroup countdownCanvas;

    [SerializeField] private float shieldDrainDelay = 3.5f;
    private float shieldDrainDelayTimer = 0f;
    private bool ShieldDrain = false;

    private HealthManagerLivesSystem healthManager;
    private int maxHealth;

    private bool healthResetOnCheckpoint = false;

    void Start()
    {
        // Locate ShieldBar by tag
        GameObject shieldBarObj = GameObject.FindGameObjectWithTag("ShieldBar");
        if (shieldBarObj != null)
        {
            shieldBar = shieldBarObj.GetComponent<ShieldBar>();
            shieldBar.SetMaxTime(shieldDuration);
        }

        // Locate TMP Text by tag "Timer"
        GameObject timerObj = GameObject.FindGameObjectWithTag("Timer");
        if (timerObj != null)
        {
            countdownText = timerObj.GetComponent<TMP_Text>();
            countdownCanvas = timerObj.GetComponent<CanvasGroup>();
            if (countdownCanvas == null)
                countdownCanvas = timerObj.AddComponent<CanvasGroup>();
            countdownCanvas.alpha = 0f;
        }
        else
        {
            Debug.LogWarning("Timer UI (TMP_Text) not found! Make sure it's tagged as 'Timer'");
        }

        // Cache health manager and max health
        healthManager = FindFirstObjectByType<HealthManagerLivesSystem>();
        if (healthManager != null)
            maxHealth = healthManager.hearts.Length;

        countdownRemaining = countdownDuration;
        ActivateShield(true); // Start with shield
    }

    void Update()
        {
        if (isOnCheckpoint && !healthResetOnCheckpoint)
        {
            HealthManagerLivesSystem.health = maxHealth;
            healthResetOnCheckpoint = true;
            RefillShieldOnCheckpoint();
        }

        



        // As long as there is a shield, it will drain over time after a delay
        else if (isShieldActive && !isOnCheckpoint)
        {
            if (ShieldDrain)
            {
                shieldDrainDelayTimer -= Time.deltaTime;
                if (shieldDrainDelayTimer <= 0f)
                {
                    ShieldDrain = false;
                }
            }
            else
            {
                shieldTimeRemaining -= Time.deltaTime;

                if (shieldTimeRemaining <= 0f)
                {
                    shieldTimeRemaining = 0f;
                    DeactivateShield();
                    StartCountdown();
                }
            }

            if (shieldBar != null)
                shieldBar.SetTime(shieldTimeRemaining);
        }

        // Timer countdown that when reaches zero, damages the player
        if (isCountdownActive && !isOnCheckpoint)
        {
            countdownRemaining -= Time.deltaTime;
            UpdateCountdownUI();

            if (countdownRemaining <= 10f && !hasFlashed)
            {
                StartCoroutine(FlashCountdownText());
                hasFlashed = true;
            }

            if (countdownRemaining <= 0f)
            {
                isCountdownActive = false;
                countdownRemaining = 0f;
                if (countdownCanvas != null)
                    countdownCanvas.alpha = 0f;

                HealthManagerLivesSystem.health--;
                Debug.Log("Timer expired: player damaged!");
            }
        }

        // Damage happens when the shield is down and timer is zero
        if (!isShieldActive && !isCountdownActive && !isOnCheckpoint)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                HealthManagerLivesSystem.health--;
                damageTimer = 0f;
                Debug.Log("Auto damage applied!");
            }
        }
        else
        {
            damageTimer = 0f;
        }
    }

    public void ActivateShield(bool fullRefill)
    {
        isShieldActive = true;

        if (fullRefill)
            shieldTimeRemaining = shieldDuration;

        if (forcefield != null)
            forcefield.SetActive(true);

        if (shieldBar != null)
        {
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(shieldTimeRemaining);
        }

        isCountdownActive = false;
        hasFlashed = false;

        if (countdownCanvas != null)
            countdownCanvas.alpha = 0f;

        shieldDrainDelayTimer = shieldDrainDelay;
        ShieldDrain = true;

        Debug.Log("Shield activated!");
    }


    public void DeactivateShield()
    {
        isShieldActive = false;

        if (forcefield != null)
            forcefield.SetActive(false);

        if (shieldBar != null)
            shieldBar.SetTime(0f);

        Debug.Log("Shield deactivated!");
    }

    public void SetShieldToFraction(float fraction)
    {
        shieldTimeRemaining = Mathf.Clamp(shieldDuration * fraction, 0f, shieldDuration);
        isShieldActive = true;

        if (forcefield != null)
            forcefield.SetActive(true);

        if (shieldBar != null)
        {
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(shieldTimeRemaining);
        }

        isCountdownActive = false;
        hasFlashed = false;

        if (countdownCanvas != null)
            countdownCanvas.alpha = 0f;

        shieldDrainDelayTimer = shieldDrainDelay;
        ShieldDrain = true;

        Debug.Log($"Shield set to {fraction * 100}% and activated!");
    }

    private void StartCountdown()
    {
        isCountdownActive = true;
        hasFlashed = false;

        if (countdownCanvas != null)
            countdownCanvas.alpha = 1f;

        UpdateCountdownUI();
        Debug.Log("Countdown started: " + countdownDuration + "s");
    }

    private void UpdateCountdownUI()
    {
        if (countdownText != null && shieldBar != null)
        {
            countdownText.enabled = shieldBar.IsEmpty();

            if (shieldBar.IsEmpty())
            {
                int minutes = Mathf.FloorToInt(countdownRemaining / 60f);
                int seconds = Mathf.FloorToInt(countdownRemaining % 60f);
                countdownText.text = $"Timer : {minutes:00}:{seconds:00}";
            }
        }
    }

    private System.Collections.IEnumerator FlashCountdownText()
    {
        while (countdownRemaining > 0f && countdownRemaining <= 10f)
        {
            if (countdownText != null)
            {
                countdownText.color = Color.red;
                yield return new WaitForSeconds(0.5f);
                countdownText.color = Color.white;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Enemy"))
            return;



        // 2) Apply knock-back impulse
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            HealthManagerLivesSystem.health--;
            Debug.Log("Collided with Enemy!");
            // Calculate direction from enemy to player
            Vector2 hitPoint = col.GetContact(0).point;
            Vector2 dir = ((Vector2)transform.position - hitPoint).normalized;
            rb.AddForce(dir * knockbackStrength, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyJumpDamaged"))
            Destroy(col.transform.parent.gameObject);
    }

    private void RefillShieldOnCheckpoint()
    {
        float halfShield = shieldDuration * 0.5f;

        if (shieldTimeRemaining <= 0f)
        {
            // If empty, refill to 50%
            shieldTimeRemaining = halfShield;
        }
        else
        {
            // If not empty, add half (but don't exceed max)
            shieldTimeRemaining = Mathf.Clamp(shieldTimeRemaining + halfShield, 0f, shieldDuration);
        }

        ActivateShield(false); // Don't set shieldTimeRemaining again in there

        if (shieldBar != null)
        {
            shieldBar.SetMaxTime(shieldDuration);
            shieldBar.SetTime(shieldTimeRemaining);
        }

        Debug.Log($"Shield refilled. Current: {shieldTimeRemaining}/{shieldDuration}");
    }

}