using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnField
    {



        [Tooltip("Where enemies will spawn")]
        public Transform spawnPoint;


        [Tooltip("Which enemy prefabs can spawn here")]
        public List<GameObject> enemyPrefabs;

    }

    [Header("Spawn Fields Configuration")]
    [Tooltip("Assign each spawn area Transform and its possible enemies")]
    [SerializeField] private List<SpawnField> spawnFields;



    [Tooltip("The enemy prefab to spawn")]
    [SerializeField] private GameObject enemyShooterPrefab;

    [Tooltip("Where the enemy will appear")]
    [SerializeField] private Transform spawnPoint1,enemyShooterSpawnPoint;


   

    private void Awake()
    {
        if (spawnFields == null || spawnFields.Count == 0)
            Debug.LogWarning("EnemySpawner: No spawn fields configured.");

        for (int i = 0; i < spawnFields.Count; i++)
        {
            var field = spawnFields[i];
            if (field.spawnPoint == null)
                Debug.LogWarning($"SpawnField[{i}] has no spawnPoint assigned.");
            if (field.enemyPrefabs == null || field.enemyPrefabs.Count == 0)
                Debug.LogWarning($"SpawnField[{i}] has no enemyPrefabs assigned.");
        }
    }

    /// Spawns one random enemy from the specified spawn field.
    public void SpawnRandomEnemyAtField(int fieldIndex)
    {
        if (!ValidateField(fieldIndex)) return;
        var field = spawnFields[fieldIndex];
        int rand = Random.Range(0, field.enemyPrefabs.Count);
        Instantiate(field.enemyPrefabs[rand], field.spawnPoint.position, field.spawnPoint.rotation);
    }

    /// Spawns every enemy prefab in the specified spawn field.
    public void SpawnAllEnemiesAtField(int fieldIndex)
    {
        if (!ValidateField(fieldIndex)) return;
        var field = spawnFields[fieldIndex];
        foreach (var prefab in field.enemyPrefabs)
            Instantiate(prefab, field.spawnPoint.position, field.spawnPoint.rotation);
    }

    /// Spawns one random enemy at each configured spawn field.
    public void SpawnAllFieldsRandom()
    {
        for (int i = 0; i < spawnFields.Count; i++)
            SpawnRandomEnemyAtField(i);
    }

    /// Spawns all enemy prefabs at each configured spawn field.
    public void SpawnAllFieldsAllEnemies()
    {
        for (int i = 0; i < spawnFields.Count; i++)
            SpawnAllEnemiesAtField(i);
    }

    /// Validates that the fieldIndex refers to a properly configured spawn field.
    private bool ValidateField(int fieldIndex)
    {
        if (spawnFields == null || fieldIndex < 0 || fieldIndex >= spawnFields.Count)
        {
            Debug.LogWarning($"EnemySpawner: fieldIndex {fieldIndex} out of range.");
            return false;
        }

        var field = spawnFields[fieldIndex];
        if (field.spawnPoint == null || field.enemyPrefabs == null || field.enemyPrefabs.Count == 0)
        {
            Debug.LogWarning($"EnemySpawner: SpawnField[{fieldIndex}] is not fully configured.");
            return false;
        }

        return true;
    }

    // Example: spawn one random enemy at each field on Start
    private void Start()
    {
        SpawnAllFieldsRandom();
    }
}
