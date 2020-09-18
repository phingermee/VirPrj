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
    //Ссылка на скрипт, описывающий взаимодействие с сюжетными предметами
    private Modes mode;
    private Camera _camera;
    private GameObject messagePanel;

    private void Start()
    {
        //Находим объект через поиск тега, поскольку игрок подгружается из префаба и... ну, в общем, возникают проблемы при попытке дать прямую ссылку
        _gameController = GameObject.FindGameObjectWithTag("GameController");
        mode = _gameController.GetComponent<Modes>();
        _camera = GetComponent<Camera>();
        messagePanel = transform.GetChild(1).gameObject;
        //Вырубаем информационную панель с подсказкой через 4 секунды после старта
        StartCoroutine(HideMessagePanel(messagePanel));
    }

    //Показываем и скрываем панель с подсказкой
    IEnumerator HideMessagePanel(GameObject messagePanel)
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
            //Смотрим, было ли столкновение луча с монтировкой (помечена тегом "Damage")
            if (Physics.Raycast(ray, out hit, 2) && hit.collider.name == "Crowbar")
            {
                hit.collider.gameObject.SetActive(false);
                crowbar.SetActive(true);
            }
            //Смотрим, было ли столкновение луча с крышкой сейфа (помечена тегом "Cap")
            else if (Physics.Raycast(ray, out hit, 2) && hit.collider.tag == "Cap")
            {
                mode.cat.GetComponent<CatBehavior>().CatBack();
                mode.SeifLockMode();
            }
            //Смотрим, было ли столкновение луча с дверью
            else if (Physics.Raycast(ray, out hit, 5) && hit.collider.tag == "Finish")
            {
                mode.DoorLockMode();
            }
            //Смотрим, было ли столкновение луча с ноутбуком (если было, то берём ноутбук в руки - включаем уже при отдельном клике на кнопку)
            else if (Physics.Raycast(ray, out hit, 14) && hit.collider.tag == "Laptop" && !mode.isLaptopModeActive)
            {
                mode.LaptopMode();
                StartCoroutine(HideMessagePanel(transform.GetChild(1).gameObject));
            }
            //Включаем ноутбук при нажатии на соответствующую кнопку
            else if (Physics.Raycast(ray, out hit, 14) && hit.collider.name == "LaptopButton")
            {
                hit.collider.gameObject.transform.Translate(0, 0.02f, 0);
                mode.LaptopOn();
                messagePanel.SetActive(true);
                messagePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Чтобы положить ноутбук на стол, нажмите TAB";
            }
            //Если луч столкнулся с иконкой текстового документа, то запускаем проверку на двойной клик: (см. дальше)
            if (Physics.Raycast(ray, out hit, 14) && hit.collider.name == "EmailIcon")
            {
                mode.TextFileClick();
            }
            //Включаем ТВ при нажатии на соответствующую кнопку
            else if (Physics.Raycast(ray, out hit, 14) && hit.collider.name == "TVButton")
            {
                mode.TVMode();
            }
        }
    }
}
