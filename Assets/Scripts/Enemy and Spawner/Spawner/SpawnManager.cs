using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("Drag in all your checkpoint Transforms here.")]
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

        currentSpawnPoint = spawnPoints[0];
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerInstance != null)
            Destroy(playerInstance);

        playerInstance = Instantiate(
            playerPrefab,
            currentSpawnPoint.position,
            currentSpawnPoint.rotation
        );
    }

    public void Respawn()
    {
        SpawnPlayer();
    }

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
