using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using Localization;

public class SettingsSave : MonoBehaviour
{
    public Toggle btnButtonOn;
    public Toggle btnMusicOn;
    public VolumeSlider setVolume;
    public void SettingsAutoSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        SettingsData settings = new SettingsData();
        settings.isButtonOn = btnButtonOn.IsActive();
        settings.isMusicOn = btnMusicOn.IsActive();
        settings.volume = setVolume.musicValue;
        settings.lng = Locale.CurrentLanguage;
        FileStream file = File.Create(Application.persistentDataPath + "/settings.qr");
        bf.Serialize(file, settings);
        file.Close();
    }

    public void SettingsAutoSet()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/settings.qr", FileMode.Open);
        SettingsData settings = (SettingsData)bf.Deserialize(file);
        setVolume.musicValue = settings.volume;
        btnButtonOn.gameObject.SetActive(settings.isButtonOn);
        btnMusicOn.gameObject.SetActive(settings.isMusicOn);
        Locale.CurrentLanguage = settings.lng;
        file.Close();
    }
}
public class SettingsData
    {
        public float volume;
        public bool isMusicOn, isButtonOn;
        public string lng;
    }