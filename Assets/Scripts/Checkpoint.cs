using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = Object.FindFirstObjectByType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("Checkpoint: No SpawnManager found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawnManager.SetCheckpoint(transform);

            PlayerColl playerColl = other.GetComponent<PlayerColl>();
            if (playerColl != null)
            {
                playerColl.ActivateShield(); // Refill shield instantly
                playerColl.isOnCheckpoint = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerColl playerColl = other.GetComponent<PlayerColl>();
            if (playerColl != null)
            {
                playerColl.isOnCheckpoint = false;
            }
        }
    }
}
