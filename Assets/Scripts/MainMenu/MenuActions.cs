using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    /// Call this from a Button (OnClick) and pass in the target scene name.
    /// Make sure that scene is added in Build Settings.

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// Call this from a Button (OnClick) to quit the game.
    /// In the editor it will stop play-mode; in build it will close the app.
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
