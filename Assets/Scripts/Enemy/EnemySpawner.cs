using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("The enemy prefab to spawn")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("Where the enemy will appear")]
    [SerializeField] private Transform spawnPoint;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner: Assign prefab & spawnPoint in inspector");
            return;
        }

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // Example: spawn once at start
    private void Start()
    {
        SpawnEnemy();
    }
}
