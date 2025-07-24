using UnityEngine;
using UnityEngine.UI;

public class HealthManagerLivesSystem : MonoBehaviour
{
    public static int health = 3;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    // ensure we only show GameOver once
    private bool _gameOverTriggered = false;

    void Update()
    {
        // ————— Draw hearts —————
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;

        // ————— Check for Game Over —————
        if (!_gameOverTriggered && health <= 0)
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
}
