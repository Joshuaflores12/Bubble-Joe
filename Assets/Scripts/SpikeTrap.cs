using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] private Transform Spawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = FindAnyObjectByType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        {
            if (collision.CompareTag("Player"))
            {
                Destroy(collision.gameObject); spawnManager.Respawn();
            }
        }
    }
}
