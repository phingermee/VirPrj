using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Modes mode;

    private void Start()
    {
        mode = GetComponent<Modes>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mode.isLaptopModeActive)
        {
            mode.PauseMenuMode();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
