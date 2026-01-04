using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void LoadDeadScene()
    {
        SceneManager.LoadScene("DeadScene");
    }
}
