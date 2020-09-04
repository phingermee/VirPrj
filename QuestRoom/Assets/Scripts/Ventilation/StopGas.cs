using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopGas : MonoBehaviour
{
    GameObject deathTimer;
    public GameObject _gameController;
    public GameObject _player;
    ParticleSystem gas;
    public bool shoulWeStopIt = false;

    void Start()
    {
        Modes mode = _gameController.GetComponent<Modes>();
        //Газ - это система частиц
        gas = transform.GetChild(0).GetComponent<ParticleSystem>();
        gas.Stop();
    }

    //Функция, которая показывает полоску жизни и перезагружает сцену, если игок "умирает"
    IEnumerator GasTimer(GameObject gasPanel)
    {
        yield return new WaitForSeconds(3f);
        for (int i = gasPanel.transform.childCount-1; i > 0; i--)
        {
            //Проверка переключателя: если игрок разбивает окно монтировкой, он становится в положение TRUE - газ выветривается, полоска жизни убирается
            if (shoulWeStopIt)
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
        if (!shoulWeStopIt)
        Application.LoadLevel(Application.loadedLevel);
    }

    //Запускаем через вентиляционную решётку смертельно опасный газ и активируем панель жизни
    public void GasIsOn()
    {
        gas.Play();
        deathTimer = Instantiate(Resources.Load("Prefabs/DeathTimer", typeof(GameObject)), _player.GetComponent<Transform>()) as GameObject;
        deathTimer.GetComponent<Canvas>().planeDistance = 0.5f;
        StartCoroutine(GasTimer(deathTimer.transform.GetChild(0).gameObject));
    }

}
