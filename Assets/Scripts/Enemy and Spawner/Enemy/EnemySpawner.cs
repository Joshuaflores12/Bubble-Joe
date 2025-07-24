using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("The enemy prefab to spawn")]
    [SerializeField] private GameObject enemyPrefab, enemyShooterPrefab;

    [Tooltip("Where the enemy will appear")]
    [SerializeField] private Transform spawnPoint,enemyShooterSpawnPoint;

    /// Call this (e.g. from Start(), or another script) to spawn one enemy.
    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner: Assign prefab & spawnPoint in inspector");
            return;
        }

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Instantiate(enemyShooterPrefab, enemyShooterSpawnPoint.position, enemyShooterSpawnPoint.rotation);

    }

    // Example: spawn once at start
    private void Start()
    {
        SpawnEnemy();
    }
}

