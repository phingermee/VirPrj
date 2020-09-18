using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

/*
 Скрипт описывает поведение онлайн-комнаты в мультиплеере: поиск второго игрока,
подключение к комнате (коридору) и запуск игры при сборе двух участников
 */
namespace PhotonTutorial.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;
        [SerializeField] private GameObject startButton = null;
        
        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPlayersPerRoom = 2;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        
        public void FindOpponent()
        {
            isConnecting = true;

            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) => PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });

        public override void OnJoinedRoom()
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if(playerCount != MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting For Opponent";
            }
            else
            {
                waitingStatusText.text = "Opponent Found";
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {            
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatusText.text = "Opponent Found";

                StartGame();
            }
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel("QuestGame");
        }
    }
}