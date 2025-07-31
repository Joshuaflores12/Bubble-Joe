using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAttackController2D : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Horizontal distance from start position for patrol")]
    public float patrolDistance = 5f;
    [Tooltip("Speed of patrolling movement")]
    public float patrolSpeed = 2f;

    [Header("Detection Settings")]
    [Tooltip("Radius within which the boss detects the player")]
    public float detectRange = 15f;
    [Tooltip("Layer mask for identifying the player")]
    public LayerMask playerLayer;

    [Header("Idle Settings")]
    [Tooltip("Time the boss stands still before each attack")]
    public float idleTime = 1f;

    [Header("Jump Attack Settings")]
    [Tooltip("Horizontal force multiplier when jumping toward the player")]
    public float jumpHorizontalForce = 3f;
    [Tooltip("Vertical force when jumping")]
    public float jumpVerticalForce = 5f;
    [Tooltip("Exact time in seconds the boss remains in jump state")]
    public float jumpDuration = 0.8f;
    [Tooltip("Damage dealt if boss collides with player during jump")]
    public int damagePerJumpHit = 1;

    [Header("Projectile Attack Settings")]
    [Tooltip("Projectile prefab to instantiate")]
    public GameObject projectilePrefab;
    [Tooltip("Spawn point for projectiles when player is to the left of boss")]
    public Transform leftProjectileSpawnPoint;
    [Tooltip("Spawn point for projectiles when player is to the right of boss")]
    public Transform rightProjectileSpawnPoint;
    [Tooltip("Number of projectiles to fire per volley")]
    public int projectileCount = 3;
    [Tooltip("Initial speed of each projectile")]
    public float projectileSpeed = 10f;
    [Tooltip("Delay between each projectile launch")]
    public float timeBetweenProjectiles = 0.2f;

    [Header("Rush Attack Settings")]
    [Tooltip("Movement speed during rush attack")]
    public float rushSpeed = 8f;
    [Tooltip("Duration of rush attack in seconds")]
    public float rushDuration = 0.5f;
    [Tooltip("Damage dealt if boss collides with player during rush")]
    public int damagePerRushHit = 1;

    private Rigidbody2D rb;
    private Transform player;
    private Coroutine attackRoutine;
    private Vector2 startPos;
    private float leftLimit;
    private float rightLimit;
    private int patrolDir = 1;
    private bool isJumping;
    private bool isRushing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        leftLimit = startPos.x - patrolDistance;
        rightLimit = startPos.x + patrolDistance;
    }

    void Update()
    {
        // Detect player within range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange, playerLayer);
        player = hits.Length > 0 ? hits[0].transform : null;

        if (player != null)
        {
            // Stop horizontal movement and start attack loop
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (attackRoutine == null)
                attackRoutine = StartCoroutine(AttackCycle());
        }
        else
        {
            // No player: stop attacks and resume patrol
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
                isJumping = isRushing = false;
                rb.linearVelocity = Vector2.zero;
            }
            Patrol();
        }
    }

    void Patrol()
    {
        // Move left/right between limits
        rb.linearVelocity = new Vector2(patrolDir * patrolSpeed, rb.linearVelocity.y);
        if (transform.position.x >= rightLimit) patrolDir = -1;
        else if (transform.position.x <= leftLimit) patrolDir = 1;

        // Flip sprite accordingly
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (patrolDir > 0 ? 1 : -1);
        transform.localScale = scale;
    }

    IEnumerator AttackCycle()
    {
        int attackIndex = 0;
        while (true)
        {
            // Idle before each attack
            yield return new WaitForSeconds(idleTime);
            if (player == null) yield break;

            switch (attackIndex)
            {
                case 0:
                    yield return JumpAttack();
                    break;
                case 1:
                    yield return ProjectileAttack();
                    break;
                case 2:
                    yield return RushAttack();
                    break;
            }

            attackIndex = (attackIndex + 1) % 3;
        }
    }

    IEnumerator JumpAttack()
    {
        if (player == null) yield break;
        isJumping = true;

        // Launch toward player's current position
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * jumpHorizontalForce, jumpVerticalForce);

        // Stay in jump for fixed duration
        yield return new WaitForSeconds(jumpDuration);

        // Reset state
        isJumping = false;
        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator ProjectileAttack()
    {
        if (player == null || projectilePrefab == null) yield break;

        // Choose correct spawn point
        Transform spawnPoint = (player.position.x < transform.position.x)
            ? leftProjectileSpawnPoint
            : rightProjectileSpawnPoint;
        if (spawnPoint == null) yield break;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector2 spawnPos = spawnPoint.position;
            Vector2 dir = ((Vector2)player.position - spawnPos).normalized;
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            Rigidbody2D prb = proj.GetComponent<Rigidbody2D>();
            if (prb != null)
                prb.linearVelocity = dir * projectileSpeed;

            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }

    IEnumerator RushAttack()
    {
        if (player == null) yield break;
        isRushing = true;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        float elapsed = 0f;
        while (elapsed < rushDuration)
        {
            rb.linearVelocity = direction * rushSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset state
        isRushing = false;
        rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only damage player during active jump or rush
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (isJumping)
            {
                HealthManagerLivesSystem.health -= damagePerJumpHit;
                isJumping = false;
            }
            else if (isRushing)
            {
                HealthManagerLivesSystem.health -= damagePerRushHit;
                isRushing = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Patrol range
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(startPos.x - patrolDistance, transform.position.y, 0), transform.position);
        Gizmos.DrawLine(new Vector3(startPos.x + patrolDistance, transform.position.y, 0), transform.position);

        // Projectile spawn points
        Gizmos.color = Color.cyan;
        if (leftProjectileSpawnPoint) Gizmos.DrawSphere(leftProjectileSpawnPoint.position, 0.1f);
        if (rightProjectileSpawnPoint) Gizmos.DrawSphere(rightProjectileSpawnPoint.position, 0.1f);
    }
}
