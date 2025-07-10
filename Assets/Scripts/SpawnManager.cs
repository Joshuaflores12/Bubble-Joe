// SpawnManager.cs
using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("Drag in all your checkpoint Transforms here (in order or arbitrary).")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("Your player prefab.")]
    [SerializeField] private GameObject playerPrefab;

    private Transform currentSpawnPoint;
    private GameObject playerInstance;

    private void Start()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("SpawnManager: No spawnPoints assigned!");
            return;
        }

        // Default to first one
        currentSpawnPoint = spawnPoints[0];
        SpawnPlayer();
    }

    /// <summary>
    /// Instantiates (or re-instantiates) the player at the current checkpoint.
    /// </summary>
    private void SpawnPlayer()
    {
        // Clean up old instance if any
        if (playerInstance != null)
            Destroy(playerInstance);

        playerInstance = Instantiate(
            playerPrefab,
            currentSpawnPoint.position,
            currentSpawnPoint.rotation
        );
    }

    /// <summary>
    /// Call this to respawn the player at the last checkpoint.
    /// </summary>
    public void Respawn()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// Switches which Transform will be used on the next spawn/respawn.
    /// The passed-in transform must be one of those in spawnPoints.
    /// </summary>
    public void SetCheckpoint(Transform checkpointTransform)
    {
        if (spawnPoints.Contains(checkpointTransform))
        {
            currentSpawnPoint = checkpointTransform;
            Debug.Log($"Checkpoint set to: {checkpointTransform.name}");
        }
        else
        {
            Debug.LogWarning($"SpawnManager: {checkpointTransform.name} not in spawnPoints list!");
        }
    }
}
