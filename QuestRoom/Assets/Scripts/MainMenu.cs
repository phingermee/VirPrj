using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Localization;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button btnContinue;
    public void StartGame()
    {
        SceneManager.LoadScene("QuestGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Continue()
    {   
        LoaderCheck.isGameLoad = true;
        SceneManager.LoadScene("QuestGame");
    }

    public void Reset()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.qr"))
        {
            File.Delete(Application.persistentDataPath + "/saves.qr");
        }
    }

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.qr"))
        {
            btnContinue.gameObject.SetActive(true);
        }
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

