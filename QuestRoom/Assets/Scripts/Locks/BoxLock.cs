//-------------------------------------
// Скрипт описывает работу с механическим замком от сейфа с монтировкой
//-------------------------------------
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Создаём класс Box, определяющий параметры и свойства для работы с отдельным барабаном замка (если делать всё в одном классе, возникают конфликты и путаница, мозг просто кипит)
class Box
{
    //Создаём список цветов, которые есть на барабанах замка
    public List<Color> colorsList = new List<Color>() {
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    //Это свойство ограничивает индекс объекта (замочный барабан) количеством возможных цветов - включительно от 0 до 3
    public int IndexOfCellColor
    {
        get { return _cellColorIndex; }
        set
        {
            _cellColorIndex = value > colorsList.Count - 1 ? 0 : value < 0 ? colorsList.Count - 1 : value;
            if (value < 0)
            {
                _cellColorIndex = colorsList.Count - 1;
            }
            else if (value > colorsList.Count - 1)
            {
                _cellColorIndex = 0;
            }
            else
            {
                _cellColorIndex = value;
            }
        }
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
    private void OpenBox()
    {
        //Запускаем анимацию крышки сейфа
        mode.seifCapAnim.Play("seifCapAnim");
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
        var rightCellsCount = activeDrumImgComponent.Where(x => x.color == Color.green).Count();
        //Если все 4 барабана повёнуты к нам зелёным бочком, то открываем ящик (сейф)
        if (rightCellsCount == 4)
        {
            //Запоминаем, что второе задание выполнено
            mode.questTasks[1] = true;
            OpenBox();
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
