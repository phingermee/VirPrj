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
        gas = transform.GetChild(0).GetComponent<ParticleSystem>();
        gas.Stop();
    }

    IEnumerator GasTimer(GameObject gasPanel)
    {
        yield return new WaitForSeconds(3f);
        for (int i = gasPanel.transform.childCount-1; i > 0; i--)
        {
            if (shoulWeStopIt)
            {
                gas.Stop();
                Destroy(deathTimer);
                break;
            }
            Image timerPoint = gasPanel.transform.GetChild(i).GetComponent<Image>();
            timerPoint.color = new Color(255, 0, 0);
            yield return new WaitForSeconds(3f);
        }
        Application.LoadLevel(Application.loadedLevel);
    }

    public void GasIsOn()
    {
        gas.Play();
        deathTimer = Instantiate(Resources.Load("Prefabs/DeathTimer", typeof(GameObject)), _player.GetComponent<Transform>()) as GameObject;
        deathTimer.GetComponent<Canvas>().planeDistance = 0.5f;
        StartCoroutine(GasTimer(deathTimer.transform.GetChild(0).gameObject));
    }

}
