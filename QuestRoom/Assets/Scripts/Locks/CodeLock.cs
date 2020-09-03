using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeLock : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip pushBtn;
    public AudioClip error;
    public AudioClip success;
    //Ссылка на крышку люка
    GameObject door;
    //Ссылка на жкран кодового замка
    Transform inputPanel;
    //Ссылка на скрипт с режимами игры
    Modes mode;
    //Загоняем код в список по циферькам
    private List<int> code = new List<int>{1,2,3};
    //Счётчик, который определяет, какой по счёту символ кода вводится
    private int index = 0;

    private void Start()
    {
        inputPanel = transform.GetChild(0);
        door = GameObject.FindGameObjectWithTag("Finish");
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();
    }

    public void Insert(int number)
    {
        //Если введённая циферька соотвествует очередному символу кода, то на экране загорается зелёный огонёк
            if (number == code[index])
            {
                audio.PlayOneShot(pushBtn);
                inputPanel.GetChild(index).GetComponent<Image>().color = new Color(0f, 255f, 0f);
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
                    inputPanel.GetChild(i).GetComponent<Image>().color = new Color(255f, 0f, 0f);
                }                
            }
    }

    //Если код успешко введён или игрок закрывает Canvas с замком, то замок удаляется со сцены
    public void Exit()
    {
        mode.iscodeLockActive = false;
        Destroy(this.gameObject);
    }

    IEnumerator OpenDoor()
    {
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        //Эффектно открываем крышку сейфа и удвигаем кодовый замок
        for (int i = 0; i < 90; i++)
        {
            transform.Translate(new Vector3(0.01f,0,0));
            door.transform.GetChild(0).transform.Rotate(new Vector3(0, 0, 1));
            yield return new WaitForFixedUpdate();
        }
        mode.isDoorOpen = true;
        Exit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }
    }
}
