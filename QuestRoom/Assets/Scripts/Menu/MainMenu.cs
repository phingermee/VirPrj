using UnityEngine;
using UnityEngine.SceneManagement;
using Localization;
using System.IO;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private DataTransfer dataControl = null;
    public Button btnContinue;
    [SerializeField]  public SettingsSave ia;

    public void StartGame()
    {
        dataControl.isItCoopGame = false;
        SceneManager.LoadScene("QuestGame");
    }

    public void StartMultplayerGame()
    {
        dataControl.isItCoopGame = true;
        SceneManager.LoadScene("MultiplayerMenu");
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

        if (File.Exists(Application.persistentDataPath + "/settings.qr"))
        {
            ia.SettingsAutoSave();
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

