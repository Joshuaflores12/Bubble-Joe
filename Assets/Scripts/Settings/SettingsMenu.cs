using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider Volume;
    public AudioMixer mainAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
      
    }

   

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void Quit_to_Main_Menu(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void SetVolume()
    {
        mainAudio.SetFloat("Volume", Volume.value);
    }

    public void Exit_to_Desktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }
}
