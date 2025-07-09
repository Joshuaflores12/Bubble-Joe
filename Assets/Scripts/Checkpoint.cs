using UnityEngine;
using Object = UnityEngine.Object;

public class Checkpoint : MonoBehaviour
{
    private SpawnManager spawnManager;

    private void Awake()
    {
        // Finds your SpawnManager (same as before)
        spawnManager = Object.FindFirstObjectByType<SpawnManager>();
        if (spawnManager == null)
            Debug.LogError("Checkpoint2D: No SpawnManager found in scene!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure your player has a Rigidbody2D + Collider2D, and is tagged "Player"
        if (other.CompareTag("Player"))
            spawnManager.SetCheckpoint(transform);
    }
}