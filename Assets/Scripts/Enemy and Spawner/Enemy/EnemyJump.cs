using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("World-space distance from start to each patrol edge")]
    [SerializeField] private float patrolDistance = 3f;
    [Tooltip("Horizontal patrol speed (units/sec)")]
    [SerializeField] private float speed = 2f;

    [Header("Jump Settings")]
    [Tooltip("Upward impulse for each jump")]
    [SerializeField] private float jumpForce = 5f;
    [Tooltip("Seconds between jumps")]
    [SerializeField] private float jumpInterval = 1f;

    [Header("Damage Settings")]
    [Tooltip("How hard the player is knocked back on hit")]
    [SerializeField] private float knockbackStrength = 5f;

    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private bool movingRight = true;

    private Rigidbody2D rb;
    private float jumpTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("EnemyJump requires a Rigidbody2D on the same GameObject.");
    }

    void Start()
    {
        // define patrol edges
        leftEdge  = transform.position + Vector3.left  * patrolDistance;
        rightEdge = transform.position + Vector3.right * patrolDistance;
    }

    void Update()
    {
        Patrol();
        HandleJumping();
    }

    private void Patrol()
    {
        Vector3 target = movingRight ? rightEdge : leftEdge;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
            movingRight = !movingRight;
    }

    private void HandleJumping()
    {
        jumpTimer += Time.deltaTime;

        // only jump if interval has passed and we're roughly on the ground
        if (jumpTimer >= jumpInterval && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
            return;

        // 1) subtract 1 health
        HealthManagerLivesSystem.health--;

        // 2) knock back the player
        var playerRb = col.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // find collision point and direction
            Vector2 hitPoint = col.GetContact(0).point;
            Vector2 dir = ((Vector2)col.transform.position - hitPoint).normalized;
            playerRb.AddForce(dir * knockbackStrength, ForceMode2D.Impulse);
        }

    }
}
