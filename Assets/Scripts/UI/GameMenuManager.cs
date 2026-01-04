using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject GameMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    public static bool isGamePaused = false; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ContinueGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        
        GameMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {

        isGamePaused = false;
        
        GameMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SettingMenu()
    {
        ButtonClickSound();

        GameMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ReturnGameMenu()
    {
        ButtonClickSound();
        
        settingsPanel.SetActive(false);
        GameMenuPanel.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        ButtonClickSound();

        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        ButtonClickSound();

        Application.Quit();
    }

    private void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}