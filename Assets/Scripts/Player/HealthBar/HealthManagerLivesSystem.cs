using UnityEngine;
using UnityEngine.UI;

public class HealthManagerLivesSystem : MonoBehaviour
{
    [SerializeField] public static int health = 3;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    void Update()
    {
        // Prevent health from going below 0
        health = Mathf.Clamp(health, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;
        }

        if (health <= 0)
        {
            Debug.Log("Game Over!");
            // Implement game over logic here (e.g., restart scene or show menu)
        }
    }
}
