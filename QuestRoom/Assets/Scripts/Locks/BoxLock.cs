using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Box
{
    //Создаём массив цветов, которые есть на барабанах замка
    public List<Color> colorsList = new List<Color>() { new Color(255, 0, 0), new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255) };
    int _cellColorIndex;

    //Это свойство ограничивает индекс объекта (замочный барабан) количеством возможных цветов - включительно от 0 до 3
    public int CellColorIndex
    {
        get
        {
            return _cellColorIndex;
        }
        set
        {
            if (value > colorsList.Count-1)
                _cellColorIndex = 0;
            else if (value < 0)
                _cellColorIndex = colorsList.Count-1;
            else _cellColorIndex = value;
        }
    }

    //Этот конструктор принимает номер прокрученного барабана
    public Box(int colind)
    {
        CellColorIndex = colind;        
    }
}

public class BoxLock : MonoBehaviour
{
    Modes mode;
    GameObject seifCap;
    //Создаём список объектов класса Box, в котором разместим ссылки на кодовые барабаны
    List<Box> lockDrums;

    void Start()
    {
        //Получаем ссылку на список игровых режимов (чтобы проверять переключатели)
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();
        //Получаем ссылку на крышку ящика
        seifCap = GameObject.FindGameObjectWithTag("Cap");
        //Создаём ссылки на кодовые барабаны и загоняем их в список
        lockDrums = new List<Box>(4);
        for (int i = 0; i < 4; i++)
        {
            lockDrums.Add(new Box(i));
        }
    }

    //Функция закрытия замка (вызывается после открытия или выхода из соотв. режима)
    public void Exit()
    {
        mode._isBoxLockActive = false;
        Destroy(this.gameObject);
    }


    //Эффектно открываем ящик
    IEnumerator OpenBox()
    {
        for (int i = 0; i < 90; i++)
        {
            transform.Translate(5, 0, 0);
            seifCap.transform.Rotate(new Vector3(-1, 0, 0));
            yield return new WaitForFixedUpdate();
        }
        Exit();
    }

    //Функция, которая отображает активный цвет на кодовом барабане
    void Display(int partNum)
    {
        //Создаём объект класса Box (чтобы получить доступ к цветам из списка colorsList, который объявлен в классе Box) и...
        Box cell = lockDrums[partNum];
        ////получаем доступ к цвету компонента Image на прокрученном только что барабане и заливаем его цветом из списка colorsList
        transform.GetChild(partNum).GetChild(0).GetComponent<Image>().color = cell.colorsList[cell.CellColorIndex];
        int count = 0;
        //Проверяем: если все барабаны повёрнуты зелёный бочком к игроку, то открываем ящик (сейф)
        for (int i = 0; i < 4; i++)
        {
            if (transform.GetChild(i).GetChild(0).GetComponent<Image>().color == cell.colorsList[2])
                count++;
        }
        if (count == 4)
        {
            StartCoroutine(OpenBox());
        }
    }

    //При нажатии на копку со стрелочкой, направленной вниз, переключаем цвет на следующий в списке colorsList
    public void ChangeColorUp(int partNum)
    {
        lockDrums[partNum].CellColorIndex--;
        Display(partNum);
    }

    //При нажатии на копку со стрелочкой, направленной вверх, переключаем цвет на предыдущий в списке colorsList
    public void ChangeColorDown(int partNum)
    {
        lockDrums[partNum].CellColorIndex++;
        Display(partNum);
    }
        
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }
    }
}
