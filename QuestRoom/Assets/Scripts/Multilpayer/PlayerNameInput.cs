﻿using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonTutorial.Menus
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button continueButton = null;
        [SerializeField] private GameObject dataControl = null;

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            nameInputField.text = defaultName;

            SetPlayerName(defaultName);
        }

        //Если поле для ввода имени НЕ пустое, кнопка запуска активируется
        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            string playerName = nameInputField.text;
            PhotonNetwork.NickName = playerName;
            
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }
    }
}