using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public static GameObject settingsPanel;
    public Slider Volume;
    public AudioMixer mainAudio;

    private void Awake()
    {
        settingsPanel = GameObject.Find("Settings_Menu");

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
      if (Time.timeScale == 0f)
      {
            Time.timeScale = 1f;
      }
     
    }

   

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        bool isActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(false);
       
        Time.timeScale = isActive ? 0f : 1f;
    }

    public void Quit_to_Main_Menu(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void SetVolume()
    {
        mainAudio.SetFloat("Volume", Volume.value);
    }

    public void Retry()
    {
        Time.timeScale = 1f;   
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        HealthManagerLivesSystem.health = 3; 
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
