using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAndHit : MonoBehaviour
{
    //Получаем доступ к списку режимов игры
    Modes mode;
    
    void Start()
    {
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();
    }

    void Update()
    {
        //Если игрок кликнул на монтировку в режиме сбора предметов, включается Режим Монтировки (позволяет разбить окно)
        if (Input.GetMouseButtonDown(0))
        {
            mode.CrowbarMode();
        }
    }
}
