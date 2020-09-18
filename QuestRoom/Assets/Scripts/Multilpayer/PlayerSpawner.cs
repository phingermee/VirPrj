using Photon.Pun;
using PhotonTutorial.Movement;
using UnityEngine;

//   Скрипт описывает подгрузку префаба игрока на игровую сцену из папки Resources
namespace PhotonTutorial
{
    public class PlayerSpawner : MonoBehaviourPun
    {
        public static FPSInput hostInput;
        [SerializeField] private GameObject playerPrefab = null;
        private DataTransfer dataControl = null;
        
        //Загружаем префаб игрока из папки Resources
        void Start()
        {
            //Получаем доступ к неубиваемому объекту, который содержит данные, переданные со стартовой сцены
            dataControl = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataTransfer>();

            //Если игрок в главном меню выбрал мyльтиплеерную игру, то подгружаем префаб в пространство PhotonNetwork
            if (dataControl.isItCoopGame)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-1, 0.5f, 1.5f), Quaternion.Euler(0, 160, 0));
            }
            else
            {   
                Vector3 loadedPos;
                if (!LoaderCheck.isGameLoad)
                {
                    loadedPos = new Vector3(-1, 0.5f, 1.5f);
                }
                else
                {
                    loadedPos = SaveLoad.LoadPosition();
                }
                hostInput = Instantiate(playerPrefab, loadedPos, Quaternion.Euler(0, 160, 0)).GetComponent<FPSInput>();
            }
        }
    }
}