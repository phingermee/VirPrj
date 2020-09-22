using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
   Скрипт принимет данные из главного меню, чтобы к ним могли обращаться скрипты из других сцен. В частности,
   здесь передаётся информация о том, какой режим игры выбрал игрок - одиночную игру или мультиплеер
 */
public class DataTransfer : MonoBehaviour
{
    public bool isItCoopGame = false;

    void Awake() => DontDestroyOnLoad(gameObject);
}
