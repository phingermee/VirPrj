using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Скрипт описывает работу с кодовым замком для двери
public class CodeLock : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip pushBtn;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip openDoor;
    //Список цветовых ячеек замка (показывают, верно ли введена очередная циферька)
    [SerializeField] private List<Image> imageOfCell = new List<Image>();
    //Ссылка на экран кодового замка
    [SerializeField] private Transform inputPanel; 
    //Ссылка на скрипт с режимами игры
    private Modes mode;
    //Загоняем код в список по циферькам
    private List<int> code = new List<int>{1,2,3};
    //Счётчик, который определяет, какой по счёту символ кода вводится
    private int index = 0;

    //Получаем доступ к режимам игры (через поиск, потому что замок подгружается из префаба)
    private void Start() => mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();

    //Если код успешко введён или игрок закрывает Canvas с замком, то замок удаляется со сцены
    public void Exit()
    {
        mode.iscodeLockActive = false;
        Destroy(gameObject);
    }

    IEnumerator OpenDoor()
    {
        //Эффектно открываем дверь
        audio.PlayOneShot(openDoor);
        for (int i = 0; i < 90; i++)
        {
            mode.door.transform.Rotate(new Vector3(0, 0, 1));
            yield return new WaitForFixedUpdate();
        }
        mode.isDoorOpen = true;
        Exit();
    }

    public void Insert(int number)
    {
        //Если введённая циферька соотвествует очередному символу кода, то на экране загорается зелёный огонёк
        if (number == code[index])
        {
            audio.PlayOneShot(pushBtn);
            imageOfCell[index].color = new Color(0f, 255f, 0f);
            index++;
            //Если при этом оказывается, что это была последняя по счёту циферька, то сейф открывается
            if (index == 3)
            {
                audio.PlayOneShot(success);
                index = 0;
                StartCoroutine(OpenDoor());
            }
        }
        //А вот если введённая циферька НЕ соотвествует очередному символу кода, то экран замка загорается красным
        else if (number != code[index])
        {
            audio.PlayOneShot(error);
            index = 0;
            for (int i = 0; i < inputPanel.childCount; i++)
            {
                imageOfCell[i].color = new Color(255f, 0f, 0f);
            }
        }
    }

    private void Update()
    {
        //При нажатии на Q выходим из режима сбора предметов и отключаем замок
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }
    }
}
