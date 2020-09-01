using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Modes mode;
    private static bool gamePause = false;
    public GameObject menuUI;
    private MouseLook yRotation, xRotation;

    private void Start()
    {
        mode = GetComponent<Modes>();
        yRotation = Camera.main.GetComponent<MouseLook>();
        xRotation = yRotation.transform.parent.GetComponent<MouseLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
