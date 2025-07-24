using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    [Tooltip("Drag your Game Over UI Panel here (inactive by default)")]
    public GameObject gameOverPanel;

    [Tooltip("Scene name to load when 'Next' is pressed")]
    public string nextSceneName;

    private bool _gameOverShown = false;

    void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Call to display the Game Over UI and pause the game.
    /// </summary>
    public void ShowGameOver()
    {
        if (_gameOverShown) return;
        _gameOverShown = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    /// <summary>
    /// Button‐handler: restart the current scene.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Button‐handler: load the configured next scene.
    /// </summary>
    public void LoadNextScene()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
