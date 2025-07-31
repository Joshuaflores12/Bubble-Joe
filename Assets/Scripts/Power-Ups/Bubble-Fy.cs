using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float lightGravityScale = 0.6f;
    [SerializeField] private float gravityDuration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpTimer timerScript = other.GetComponent<PowerUpTimer>();
            if (timerScript != null)
            {
                timerScript.StartGravityPowerUp(lightGravityScale, gravityDuration);
                Debug.Log("Bubble triggered gravity power-up!");
            }

            Destroy(gameObject); 
        }
    } 
}
