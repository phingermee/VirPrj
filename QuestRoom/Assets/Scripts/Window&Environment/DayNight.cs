using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    //Начальная длительность дня (влияет на скорость движения солнца)
    float daySpeed = 10f;
    //Объект, вокруг которого крутится солнце
    public GameObject centerPoint;
    //Само солнце (в нашем случае - источник света Point)
    public GameObject sun;
    //Звёзды в виде партиклов
    public GameObject stars;
    //Лампа, которая включается с наступлением ночи
    public GameObject bulb;
    //Переключатель "день-ночь", который позволяет избежать постоянного запуска/остановки генерации звёзд
    bool isDayNow;
    //Дополнительные источники света, которые делают освещение комнаты более натуральным
    public Light lightOne;
    public Light lightTwo;

    void Update()
    {
        Vector3 center = centerPoint.transform.position;
        //Яркость солнца (а если конкретнее, то всего освещения на сцене) зависит от его положения в небе
        lightOne.intensity = sun.transform.position.y / 15;
        lightTwo.intensity = sun.transform.position.y / 15;
        //Солнышко кру-у-утится, юху!
        sun.transform.RotateAround(center, -Vector3.left, daySpeed * Time.deltaTime);

        //Если солнце поднимается выше 4 по у, то останавливаем генерацию звёзд (они потом сами гаснут, что есть весьма реалистично)
        if (sun.transform.position.y >= 5 && !isDayNow)
        {
            daySpeed = 5;
            isDayNow = true;
            stars.GetComponent<ParticleSystem>().Stop();
            //Выключаем лампочку
            bulb.GetComponent<Light>().range = 0;
            Debug.Log("DAY");
        }
        else if (sun.transform.position.y < 5 && isDayNow)
        {
            daySpeed = 5;
            isDayNow = false;
            stars.GetComponent<ParticleSystem>().Play();
            //Включаем лампочку
            bulb.GetComponent<Light>().range = 8;
            Debug.Log("NIGHT");
        }
    }
}
