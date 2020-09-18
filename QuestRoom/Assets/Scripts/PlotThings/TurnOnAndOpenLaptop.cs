using System.Collections;
using UnityEngine;

// Скрипт описывает перемещение и повороты ноутбука при работе с ним (когда игрок берёт ноутбук в руки)
public class TurnOnAndOpenLaptop : MonoBehaviour
{
    public GameObject _laptopStartPanel;
    public GameObject _textFilePanel;
    public bool _doubleClick = false;
    public bool isLaptotOn = false;

    [SerializeField] private GameObject gameController;

    //Тут активируется панель текстового файла и закрывается рабочий стол
    public void openTextFile()
    {
        _textFilePanel.SetActive(true);
        _laptopStartPanel.SetActive(false);        
    }

    //Корутин, который "держит в памяти" первый клик в течение 1 секунды (реализация открытия текстового файла при двойном клике)
    public IEnumerator DoubleClick()
    {        
        yield return new WaitForSeconds(1f);
        _doubleClick = false;
    }

    /*Эта функция включает и выключает ноутбук (не путать с активацией/деактивацией Laptop Mode!!)
    в зависимости от значения переменной isLaptotOn ("включён ли ноутбук?" - true/false)*/
    public void TurnOnOff()
    {
        _laptopStartPanel.SetActive(!isLaptotOn);
        _textFilePanel.SetActive(isLaptotOn);
        isLaptotOn = !isLaptotOn;
    }

    public void IconClick()
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
