using UnityEngine;

public class EnemyProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private float direction = 1f;

    // Direction of the projectile
    public void SetDirection(float dir)
    {
        direction = dir;
    }

    // moving only on the x axis but right as its starting position
    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision with player damages player and destroys projectile
        if (collision.CompareTag("Player")) 
        {
            HealthManagerLivesSystem.health--;
            Destroy(gameObject);
        }
    }
}
