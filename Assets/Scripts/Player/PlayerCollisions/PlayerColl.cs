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

        ActivateShield(); // Start with shield
    }

    void Update()
    {
        // If shield is not active and player is on checkpoint, recharge it slowly
        if (!isShieldActive && isOnCheckpoint)
        {
            shieldTimeRemaining += Time.deltaTime;
            if (shieldTimeRemaining >= shieldDuration)
            {
                shieldTimeRemaining = shieldDuration;
                ActivateShield();
            }

            if (shieldBar != null)
                shieldBar.SetTime(shieldTimeRemaining);
        }
        // If shield is active and not on checkpoint, drain it
        else if (isShieldActive && !isOnCheckpoint)
        {
            shieldTimeRemaining -= Time.deltaTime;

            if (shieldTimeRemaining <= 0f)
            {
                shieldTimeRemaining = 0f;
                DeactivateShield();
                StartCountdown();
            }

            if (shieldBar != null)
                shieldBar.SetTime(shieldTimeRemaining);
        }

        // Countdown logic - only runs when NOT on checkpoint
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

        // Auto damage when both shield and countdown are down
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

        isCountdownActive = false;
        countdownRemaining = countdownDuration;
        hasFlashed = false;

        if (countdownCanvas != null)
            countdownCanvas.alpha = 0f;

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

    private void StartCountdown()
    {
        isCountdownActive = true;
        countdownRemaining = countdownDuration;
        hasFlashed = false;

        if (countdownCanvas != null)
            countdownCanvas.alpha = 1f;

        UpdateCountdownUI();
        Debug.Log("Countdown started: " + countdownDuration + "s");
    }

    private void UpdateCountdownUI()
    {
        if (countdownText != null)
        {
            int minutes = Mathf.FloorToInt(countdownRemaining / 60f);
            int seconds = Mathf.FloorToInt(countdownRemaining % 60f);
            countdownText.text = $"Timer : {minutes:00}:{seconds:00}";
            countdownText.color = Color.white;
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

        // 1) Subtract one life
        HealthManagerLivesSystem.health--;

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
}

