using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManagerLivesSystem : MonoBehaviour
{
    public static int health = 3; // Hearts per life
    public static int lives = 3;  // Total lives

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private TMP_Text Lives; 

    private bool _gameOverTriggered = false;

    void Start()
    {
        UpdateLivesUI();
    }

    void Update()
    {
        // ————— Draw hearts —————
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;

        // ————— Update lives UI —————
        UpdateLivesUI();

        // ————— Check for heart depletion —————
        if (health <= 0 && lives > 1)
        {
            lives--;
            health = hearts.Length; // Reset hearts for the next life
        }

        // ————— Check for Game Over —————
        if (!_gameOverTriggered && health <= 0 && lives <= 1)
        {
            _gameOverTriggered = true;

            // 1) Show your Game Over UI
            var go = FindFirstObjectByType<GameOverScene>();
            if (go != null)
                go.ShowGameOver();
            else
                Debug.LogWarning("No GameOverScene found!");

            // 2) Disable player movement & collision
            var mv = FindFirstObjectByType<Movement>();
            if (mv != null) mv.enabled = false;

            var pc = FindFirstObjectByType<PlayerColl>();
            if (pc != null) pc.enabled = false;
        }
    }

    private void UpdateLivesUI()
    {
        if (Lives != null)
            Lives.text =  lives.ToString();
    }
}
