using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Localization;


public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void SetEnglish()
    {
        LocalizeBase.SetCurrentLanguage(SystemLanguage.English);
    }

    public void SetRussian()
    {
        LocalizeBase.SetCurrentLanguage(SystemLanguage.Russian);
    }

}

