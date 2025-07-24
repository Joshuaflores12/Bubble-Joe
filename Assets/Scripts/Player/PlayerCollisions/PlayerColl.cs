// PlayerColl.cs
using UnityEngine;

public class PlayerColl : MonoBehaviour
{
    [Tooltip("How hard the player is pushed back when hit by an enemy")]
    [SerializeField] private float knockbackStrength = 5f;

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
            // Calculate direction from enemy to player
            Vector2 hitPoint = col.GetContact(0).point;
            Vector2 dir = ((Vector2)transform.position - hitPoint).normalized;
            rb.AddForce(dir * knockbackStrength, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // keeps your "stomp to kill" logic intact
        if (col.CompareTag("EnemyJumpDamaged"))
            Destroy(col.transform.parent.gameObject);
    }
}

