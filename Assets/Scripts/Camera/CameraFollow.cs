using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineCamera playerCam;
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCam = FindFirstObjectByType<CinemachineCamera>();
        player = GameObject.FindGameObjectWithTag("Player");


        playerCam.Follow = player.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerCam.Follow = player.transform;

        }
           
    }
}
