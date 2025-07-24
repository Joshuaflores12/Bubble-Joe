using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Tooltip("Impulse strength applied to the player on hit")]
    [SerializeField] private float knockbackStrength = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // 1) Deal 1 damage
        HealthManagerLivesSystem.health--;

        // 2) Apply knockback
        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // direction from spike to player
            Vector2 dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            rb.AddForce(dir * knockbackStrength, ForceMode2D.Impulse);
        }
    }
}
