using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnAndOpen : MonoBehaviour
{
    public GameObject gameController;
    Modes mode;
    public GameObject _laptopStartPanel;
    public GameObject _textFilePanel;
    public GameObject _camera;
    private bool _doubleClick = false;
    public bool isLaptotOn = false;

    private void Start()
    {
        mode = gameController.GetComponent<Modes>();
    }

    //Тут активируется панель текстового файла и закрывается рабочий стол
    public void openTextFile()
    {
        _textFilePanel.SetActive(true);
        _laptopStartPanel.SetActive(false);        
    }

    //Корутин, который "держит в памяти" первый клик в течение 1 секунды (реализация открытия текстового файла при двойном клике)
    IEnumerator DoubleClick()
    {        
        yield return new WaitForSeconds(1f);
        _doubleClick = false;
    }

    /*Эта функция включает и выключает ноутбук (не путать с активацией/деактивацией Laptot Mode!!)
    в зависимости от значения переменной isLaptotOn ("включён ли ноутбук?" - true/false)*/
    void TurnOnOff()
    {
        if (!isLaptotOn)
        {
            _laptopStartPanel.SetActive(true);
            _textFilePanel.SetActive(false);
            isLaptotOn = true;
        }
        else
        {
            _laptopStartPanel.SetActive(false);
            _textFilePanel.SetActive(false);
            isLaptotOn = false;
        }
    }

    void Update()
    {
        //Если ноут выключн, то он включится при нажатии на любую клавишу
        if (Input.anyKeyDown && mode.isLaptopModeActive && !isLaptotOn)
            TurnOnOff();

        //Если при активированном и включённом ноуте мы куда-то кликнули, то...
        if (Input.GetMouseButtonDown(0))
        {
            //..пускаем луч на координаты курсора
            Ray ray = _camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Если луч столкнулся с иконкой текстового документа, то запускаем проверку на двойной клик: (см. дальше)
            if (Physics.Raycast(ray, out hit, 14) && hit.collider.name == "EmailIcon")
            {
                //Если первый клик уже был, то открываем текстовый файл
                if (_doubleClick)
                {
                    _laptopStartPanel.SetActive(false);
                    _textFilePanel.SetActive(true);
                }
                //А если первого клика не было, то мы его фиксируем и запускаем корутин, который "держит в памяти" первый клик в течение 1 секунды
                else
                {
                    _doubleClick = true;
                    StartCoroutine(DoubleClick());
                }
            }
        }
    }
}
