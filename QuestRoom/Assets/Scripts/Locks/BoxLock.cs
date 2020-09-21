using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Скрипт описывает работу с механическим замком от сейфа с монтировкой
enum drumColors
{
    red,
    yellow,
    green,
    blue
}

//Создаём класс Box, определяющий параметры и свойства для работы с отдельным барабаном замка (если делать всё в одном классе, возникают конфликты и путаница, мозг просто кипит)
class Box
{
    //Создаём список цветов, которые есть на барабанах замка
    public List<Color> colorsList = new List<Color>() {
        //красный
        new Color(255, 0, 0),
        //жёлтый
        new Color(255, 255, 0),
        //зелёный
        new Color(0, 255, 0),
        //синий
        new Color(0, 0, 255)
    };

    //Это свойство ограничивает индекс объекта (замочный барабан) количеством возможных цветов - включительно от 0 до 3
    public int IndexOfCellColor
    {
        get { return _cellColorIndex; }
        set {  _cellColorIndex = value > colorsList.Count - 1 ? 0 : value < 0 ? colorsList.Count - 1 : value; }
    }
    private int _cellColorIndex;

    //Этот конструктор принимает номер прокрученного барабана
    public Box(int colind) => IndexOfCellColor = colind;
}

public class BoxLock : MonoBehaviour
{
    //Создаём список, в котором размещаются ссылки на лицевые панельки всех барабанов замка
    [SerializeField]  private List<Image> activeDrumImgComponent = new List<Image>();
    //Получаем доступ к списку режимов игры
    private Modes mode;

    //Создаём список объектов класса Box, в котором разместим ссылки на кодовые барабаны
    private List<Box> lockDrumsSouls;

    void Start()
    {
        //Получаем ссылку на список игровых режимов (чтобы проверять переключатели) - замок подгружается из префаба, поэтому прямую ссылку поставить не получается
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();

        //Создаём "души" кодовых барабанов и загоняем их в список
        lockDrumsSouls = new List<Box>();

        for (int i = 0; i < 4; i++)
        {
            lockDrumsSouls.Add(new Box(i));
        }
    }

    //Функция закрытия/уничтожения замка (вызывается после открытия или выхода из соотв. режима)
    public void Exit()
    {
        mode._isBoxLockActive = false;
        Destroy(gameObject);
    }

    //Эффектно открываем ящик
    IEnumerator OpenBox()
    {
        for (int i = 0; i < 90; i++)
        {
            //Сдвигаем замок в сторону
            transform.Translate(5, 0, 0);
            //Обращаемся к крышке сейфа через ссылку, размещённую в скрипте Modes, и открываем её (крышку)
            mode.seifCap.transform.Rotate(new Vector3(-1, 0, 0));
            yield return new WaitForFixedUpdate();
        }
        Exit();
    }

    //Функция, которая отображает активный цвет на кодовом барабане
    void Display(int partNum)
    {
        //Создаём объект класса Box (чтобы получить доступ к цветам из списка colorsList, который объявлен в классе Box), а затем...
        Box drum = lockDrumsSouls[partNum];
        //..получаем доступ к цвету компонента Image на прокрученном только что барабане и заливаем его цветом из списка colorsList (номер цвета передаём через поле OnClick)
        activeDrumImgComponent[partNum].color = drum.colorsList[drum.IndexOfCellColor];

        //Считаем число барабанов, повёрнутых к игроку зелёным бочком
        var rightCellsCount = activeDrumImgComponent.Where(x => x.color == drum.colorsList[(int)drumColors.green]).Count();
        //Если все 4 барабана повёнуты к нам зелёным бочком, то открываем ящик (сейф)
        if (rightCellsCount == 4)
        {
            //Запоминаем, что второе задание выполнено
            mode.questTasks[1] = true;
            StartCoroutine(OpenBox());
        }
    }

    //При нажатии на копку со стрелочкой, направленной вниз, переключаем цвет на следующий в списке colorsList
    public void ChangeColorUp(int partNum)
    {
        lockDrumsSouls[partNum].IndexOfCellColor--;
        Display(partNum);
    }

    //При нажатии на копку со стрелочкой, направленной вверх, переключаем цвет на предыдущий в списке colorsList
    public void ChangeColorDown(int partNum)
    {
        lockDrumsSouls[partNum].IndexOfCellColor++;
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
