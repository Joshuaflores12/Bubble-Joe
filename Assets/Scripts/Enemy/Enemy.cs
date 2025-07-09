using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("World-space distance from start to each patrol edge")]
    [SerializeField] private float patrolDistance = 3f;

    [Tooltip("Speed of movement in units/sec")]
    [SerializeField] private float speed = 2f;

    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private bool movingRight = true;

    private void Start()
    {
        // define patrol bounds based on where the enemy started
        leftEdge  = transform.position + Vector3.left  * patrolDistance;
        rightEdge = transform.position + Vector3.right * patrolDistance;
    }

    private void Update()
    {
        Vector3 target = movingRight ? rightEdge : leftEdge;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
            movingRight = !movingRight;
    }
}
