using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SettingMenu()
    {
        ButtonClickSound();

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        ButtonClickSound();

        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
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
