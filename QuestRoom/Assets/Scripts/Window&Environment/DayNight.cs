using UnityEngine;

// Скрипт описывает движение и яркость "Солнца" и других источников света на сцене, а также активирует/деактивирует генерацию звёзд
public class DayNight : MonoBehaviour
{
    //Объект, вокруг которого крутится солнце
    [SerializeField] private GameObject centerPoint;
    //Само солнце (в нашем случае - источник света Point)
    [SerializeField] private GameObject sun;
    //Лампа, которая включается с наступлением ночи
    [SerializeField] private Light bulbLight;    
    //Дополнительные источники света, которые делают освещение комнаты более натуральным
    [SerializeField] private Light lightOne;
    [SerializeField] private Light lightTwo;
    //Звёзды в виде партиклов
    [SerializeField] private ParticleSystem stars;
    private float anglyByOneSec;

    //Начальная длительность дня (влияет на скорость движения солнца)
    public float daySpeed;
    //Переключатель "день-ночь", который позволяет избежать постоянного запуска/остановки генерации звёзд
    private bool isDayNow;

    private void Start()
    {
        //Запускаем генерацию звёзд
        stars.Play();
        anglyByOneSec = 360f / (43200f);
    }

    void Update()
    {
        Vector3 center = centerPoint.transform.position;
        //Яркость солнца (а если конкретнее, то всего освещения на сцене) зависит от его положения в небе
        lightOne.intensity = sun.transform.position.y / 20;
        lightTwo.intensity = sun.transform.position.y / 20;
        //Солнышко кру-у-утится, юху!
        sun.transform.RotateAround(center, -Vector3.left, anglyByOneSec * Time.deltaTime * daySpeed);

        //Если солнце поднимается выше 4 по у, то останавливаем генерацию звёзд (они потом сами гаснут, что есть весьма реалистично)
        if (sun.transform.position.y >= 5 && !isDayNow)
        {
            isDayNow = true;
            stars.Stop();
            //Выключаем лампочку
            bulbLight.range = 0;
        }
        //Если солнце опускается ниже 5 по у, то запускаем генерацию звёзд
        else if (sun.transform.position.y < 5 && isDayNow)
        {
            isDayNow = false;
            stars.Play();
            //Включаем лампочку
            bulbLight.range = 8;
        }
    }
}
