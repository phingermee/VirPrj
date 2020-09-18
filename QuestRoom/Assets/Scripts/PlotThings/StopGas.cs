using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Скрипт, описывающий генерацию смертельного газа и его исчезновение при открытом окне
public class StopGas : MonoBehaviour
{
    public bool shouldWeStopIt = false;
    private GameObject deathTimer;
    private ParticleSystem gas;    

    void Start()
    {
        //Газ - это система частиц
        gas = transform.GetChild(0).GetComponent<ParticleSystem>();
        gas.Stop();
    }

    //Функция, которая показывает полоску жизни и перезагружает сцену, если игрок "умирает"
    IEnumerator GasTimer(GameObject gasPanel)
    {
        yield return new WaitForSeconds(3f);
        for (int i = gasPanel.transform.childCount-1; i > 0; i--)
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
                Image timerPoint = gasPanel.transform.GetChild(i).GetComponent<Image>();
                timerPoint.color = new Color(255, 0, 0);
                yield return new WaitForSeconds(3f);
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
        deathTimer = Instantiate(Resources.Load("Prefabs/DeathTimer") as GameObject);
        deathTimer.GetComponent<Canvas>().planeDistance = 0.5f;
        StartCoroutine(GasTimer(deathTimer.transform.GetChild(0).gameObject));
    }

}
