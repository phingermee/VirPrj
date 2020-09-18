using UnityEngine;
using Photon.Pun;

//   Скрипт описывает движение игрока
namespace PhotonTutorial.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [AddComponentMenu("Control Script/FPS Input")]
    public class FPSInput : MonoBehaviourPun
    {
        //Вводим выключатель (умножаем на него скорость всех перемещений)
        public int movingAbility = 1;

        [SerializeField] private GameObject _camera = null;

        //Определяем скорость движения персонажа
        private float speed = 9.0f;
        //Сила гравитации, которая действует на персонажа
        private float gravity = -9.8f;
        //Создаём переменную, через которую можно получить доступ к Character Controller
        private CharacterController _charController;
        //Создаём ссылку на объект DataController, к скриптам которого мы будем обращаться
        private DataTransfer dataControl = null;
        

        void Start()
        {
            //Определяем переменную, через которую можно получить доступ к Character Controller
            _charController = GetComponent<CharacterController>();
            //mode = GetComponent<Modes>();
            //Отключаем курсор, чтоб не мелькал перед глазами
            Cursor.visible = false;
            // Получаем доступ к данным, переданным со стартовой сцены (повесил их на неубиваемый объект)
            dataControl = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataTransfer>();
            if (dataControl.isItCoopGame)
            {
                _camera.SetActive(photonView.IsMine);
            }
        }

        void movingDescription()
        {
            //Определяем движение параллельно осям
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * speed * movingAbility, 0, Input.GetAxis("Vertical") * speed * movingAbility);
            //Ограничим движение по диагонали той же скоростью, что и движение параллельно осям.
            movement = Vector3.ClampMagnitude(movement, speed * movingAbility);
            //Включаем гравитацию, чтобы персонаж не летал
            movement.y = gravity;
            //Сглаживаем движение
            movement *= Time.deltaTime;
            //Преобразуем вектор движения от локальных к глобальным координатам.
            movement = transform.TransformDirection(movement);
            //Заставим этот вектор перемещать компонент CharacterController
            _charController.Move(movement * movingAbility);
        }

        public void SetPosition(Vector3 newPos)
        {
            _charController.enabled = false;
            this.gameObject.transform.position = newPos;
            _charController.enabled = true;
        }

        //Если игрок в главном меню выбрал мультиплерную игру, то осуществляем проверку принадлежности префаба этому игроку
        void Update()
        {
            if (dataControl.isItCoopGame)
            {
                if (photonView.IsMine)
                {
                    movingDescription();
                }
            }
            else
            {
                movingDescription();
            }
        }
    }
}