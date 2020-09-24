using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Проверка на столкновение Raycast-луча с сюжетными предметами и запуск соответствующих режимов из скрипта Modes
public class TakeThisThing : MonoBehaviour
{
    //Ссылка на пустой объект, к которому привязана куча скриптов, описывающих поведение различных объектов на сцене
    private GameObject _gameController;

    [SerializeField] private GameObject _player = null;
    [SerializeField] private GameObject crowbar = null;
    [SerializeField] private GameObject messagePanel = null;
    [SerializeField] private Text messageText = null;
    //Ссылка на скрипт, описывающий взаимодействие с сюжетными предметами
    private Modes mode;
    private Camera _camera;
    //Наименования сюжетных объектов, с которыми может столкнуться луч (используем, чтобы не генерировать строковый мусор при проверке соответствия имён)
    const string crowbarName = "Crowbar";
    const string seifName = "Cap";
    const string doorName = "Finish";
    const string laptopName = "Laptop";
    const string laptopButtonName = "LaptopButton";
    const string emailName = "EmailIcon";
    const string TVButtonName = "TVButton";

    private void Start()
    {
        //Находим объект через поиск тега, поскольку игрок подгружается из префаба и... ну, в общем, возникают проблемы при попытке дать прямую ссылку
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        mode = _gameController.GetComponent<Modes>();
        //Вырубаем информационную панель с подсказкой через 4 секунды после старта
        StartCoroutine(HideMessagePanel());
        _camera = this.gameObject.GetComponent<Camera>();
    }

    //Показываем и скрываем панель с подсказкой (в игре этот корутин используется несколько раз для разных подсказок)
    IEnumerator HideMessagePanel()
    {
        yield return new WaitForSeconds(3.8f);
        messagePanel.SetActive(false);
    }

    private void Update()
    {
        //Переключаемся в режим сбора предметов нажатием на Q (если, конечно, мы не работаем с вводом текста на ноутбуке)
        if (Input.GetKeyDown(KeyCode.Q) && !mode.isLaptopModeActive)
        {
            mode.ClickMode();
        }

        //Если игрок кликнул мышкой в режиме сбора предметов, запускаем проверку на столкновение с сюжетным предметом
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

           if (Physics.Raycast(ray, out hit, 2))
            {
                //Смотрим, было ли столкновение луча с монтировкой (помечена тегом "Damage")
                if (hit.collider.name == crowbarName)
                {
                    hit.collider.gameObject.SetActive(false);
                    crowbar.SetActive(true);
                }
                //Смотрим, было ли столкновение луча с крышкой сейфа (помечена тегом "Cap") и выполнено л первое задание
                else if (hit.collider.tag == seifName && mode.questTasks[0])
                {
                    //Очень сложное обращение - только ради того, чтобы не искать кота через Find
                    mode.cat.GetComponent<CatBehavior>().CatBack();
                    mode.SeifLockMode();
                }
                //Смотрим, было ли столкновение луча с дверью
                else if (hit.collider.name == doorName)
                {
                    mode.DoorLockMode();
                }
                //Смотрим, было ли столкновение луча с ноутбуком (если было, то берём ноутбук в руки - включаем уже при отдельном клике на кнопку)
                else if (hit.collider.name == laptopName && !mode.isLaptopModeActive)
                {
                    mode.LaptopMode();
                }
                //Включаем ноутбук при нажатии на соответствующую кнопку
                else if (hit.collider.name == laptopButtonName)
                {
                    mode.LaptopButtonAnim.Play("LaptopButtonAnim");
                    mode.LaptopOn();
                    messagePanel.SetActive(true);
                    messageText.text = "Чтобы положить ноутбук на стол, нажмите TAB";
                    StartCoroutine(HideMessagePanel());
                }
                //Если луч столкнулся с иконкой текстового документа, то запускаем проверку на двойной клик: (см. дальше)
                else if (hit.collider.name == emailName)
                {
                    mode.TextFileClick();
                }
                //Включаем ТВ при нажатии на соответствующую кнопку
                else if (hit.collider.name == TVButtonName)
                {
                    mode.TVMode();
                }
            }
        }
    }
}
