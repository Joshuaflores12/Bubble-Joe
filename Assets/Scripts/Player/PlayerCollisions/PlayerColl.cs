using UnityEngine;

public class PlayerColl : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthManagerLivesSystem.health--;
            //if health is <=0 game over logic here
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyJumpDamaged")) 
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
