using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public GameObject settingsPanel, Main_menuPanel;
    public Slider Volume;
    public AudioMixer mainAudio;

    private void Start()
    {
        settingsPanel.SetActive(false);
        Main_menuPanel.SetActive(true);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void Open_Settings()
    {

        settingsPanel.SetActive(true);
        Main_menuPanel.SetActive(false);
    }

    public void Close_Settings()
    {

        settingsPanel.SetActive(false);
        Main_menuPanel.SetActive(true);
    }

    public void SetVolume()
    {
        mainAudio.SetFloat("Volume", Volume.value);
    }
}
