using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, описывающий генерацию смертельного газа и его исчезновение при открытом окне
public class StopGas : MonoBehaviour
{
    public bool shouldWeStopIt = false;
    [SerializeField] private List<Image> lifeScale = new List<Image>();
    [SerializeField] private GameObject deathTimer = null;
    [SerializeField] private ParticleSystem gas;

    //Функция, которая показывает полоску жизни и перезагружает сцену, если игрок "умирает"
    IEnumerator GasTimer()
    {
        for (int i = lifeScale.Count-1; i > 0; i--)
        {
            //Проверка переключателя: если игрок разбивает окно монтировкой, он становится в положение TRUE - газ выветривается, полоска жизни убирается
            if (shouldWeStopIt)
            {
                gas.Stop();
                Destroy(deathTimer);
                break;
            }
            //Если переключатель всё ещё в положении FALSE (окно НЕ разбито), то поочерёдно окрашиваем ячейки жизни в красный
            else
            {
                lifeScale[i].color = Color.red;
                yield return new WaitForSeconds(2.2f);
            }
        }
        //Если полоска жизни на нуле, а газ всё ещё не выключен, перезагружаем сцену
        if (!shouldWeStopIt)
        Application.LoadLevel(Application.loadedLevel);
    }

    //Запускаем через вентиляционную решётку смертельно опасный газ и активируем панель жизни
    public void GasIsOn()
    {
        gas.Play();
        deathTimer.SetActive(true);
        deathTimer.transform.SetParent(Camera.main.gameObject.transform);
        StartCoroutine(GasTimer());
    }

}
