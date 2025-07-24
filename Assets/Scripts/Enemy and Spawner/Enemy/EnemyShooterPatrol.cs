using UnityEngine;
using System.Collections;

public class EnemyShooterPatrol : MonoBehaviour
{
    [Tooltip("World-space distance from start to each patrol edge")]
    [SerializeField] private float patrolDistance = 3f;

    [Tooltip("Speed of movement in units/sec")]
    [SerializeField] private float speed = 2f;

    [Tooltip("Projectile prefab to shoot")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("Where the projectile spawns from")]
    [SerializeField] private Transform firePoint;

    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private bool movingRight = true;
    private bool isWaiting = false;

    private void Start()
    {
        leftEdge = transform.position + Vector3.left * patrolDistance;
        rightEdge = transform.position + Vector3.right * patrolDistance;
    }

    private void Update()
    {
        if (isWaiting) return;

        Vector3 target = movingRight ? rightEdge : leftEdge;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            StartCoroutine(WaitAndShoot());
        }
    }

    private IEnumerator WaitAndShoot()
    {
        isWaiting = true;

        // wait for a random time between 1 and 2 seconds
        float waitTime = Random.Range(1f, 2f);
        yield return new WaitForSeconds(waitTime);

        // shoot projectile
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            float direction = movingRight ? 1f : -1f;
            projectile.GetComponent<EnemyProjectileMovement>().SetDirection(direction);
        }

        movingRight = !movingRight;
        isWaiting = false;
    }
}
