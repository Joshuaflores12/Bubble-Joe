using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject playerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
    }

    public void Respawn() 
    {
        Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
    }
}
