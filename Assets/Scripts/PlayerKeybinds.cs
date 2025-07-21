using UnityEngine;

public class PlayerKeybinds : MonoBehaviour
{
    public GameObject settingsPanel;

   

    void Start()
    {
        settingsPanel = SettingsMenu.settingsPanel;
        if (settingsPanel != null )
            {
            settingsPanel.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel != null)
        {
            bool isActive = !settingsPanel.activeSelf;
            settingsPanel.SetActive(isActive);


            Time.timeScale = isActive ? 0f : 1f;
        }


     





    }
}
