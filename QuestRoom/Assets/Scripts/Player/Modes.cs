using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class Modes : MonoBehaviour
{
    bool isClickModeActive = false;
    bool isPauseActive = false;
    bool isTVActive = false;
    public bool isGasActive = false;
    public bool isSeifOpen = false;
    public bool isDoorOpen = false;
    public bool iscodeLockActive = false;
    public bool _isBoxLockActive = false;
    public bool isLaptopModeActive = false;
    public GameObject _player;
    public GameObject _camera;
    public GameObject laptop;
    public GameObject ventilationGrid;
    public GameObject menuUI;
    public GameObject TV;
    public Vector3 oldPosition;

    //Режим сбора предметов (игрок "заморожен", курсор активен)
    public void ClickMode()
    {
        FPSInput moving = _player.GetComponent<FPSInput>();
        MouseLook _rotation = _camera.GetComponent<MouseLook>();

        if (isClickModeActive)
        {
            isClickModeActive = !isClickModeActive;
            moving.movingAbility = 1;
            _rotation.rotationAbility = 1;
            Cursor.visible = false;
        }
        else
        {
            isClickModeActive = !isClickModeActive;
            moving.movingAbility = 0;
            _rotation.rotationAbility = 0;
            Cursor.visible = true;
        }
    }

    //Активно меню паузы
    public void PauseMenuMode()
    {
        if (isPauseActive)
        {
            isPauseActive = false;
            menuUI.SetActive(false);
            Time.timeScale = 1f;
            ClickMode();
        }
        else
        {
            isPauseActive = true;
            menuUI.SetActive(true);
            Time.timeScale = 0f;
            ClickMode();
        }
    }

    //Режим монтировки (монтировка в руках, можно разрушить окно)
    public void CrowbarMode()
    {
        GameObject crowbar = GameObject.FindGameObjectWithTag("Damage");
        GameObject weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint");
        crowbar.transform.SetParent(_player.GetComponent<Transform>());
        crowbar.transform.position = weaponPoint.transform.position;
        crowbar.transform.rotation = weaponPoint.transform.rotation;
    }

    //Режим громкой улицы (когда окно разбито)
    public void LoudStreetMode()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        float musicVolume = 0.5f;
        audioSrc.volume = musicVolume;
        //Если активен режим газа, то после разбития окна газ выветривается
        if (isGasActive)
        {
            GasMode();
        }
    }

    //Режим работы с кодовым замком на ящике (сейфе)
    public void SeifLockMode()
    {
        if (!_isBoxLockActive && !isSeifOpen)
        {
            GameObject codeLock = Instantiate(Resources.Load("Prefabs/BoxLock", typeof(GameObject)), _player.GetComponent<Transform>()) as GameObject;
            _isBoxLockActive = true;
        }
    }

    //Режим работы с кодовым замком на двери
    public void DoorLockMode()
    {
        if (!iscodeLockActive && !isDoorOpen)
        {
            GameObject codeLock = Instantiate(Resources.Load("Prefabs/CodeLock", typeof(GameObject)), _player.GetComponent<Transform>()) as GameObject;
            iscodeLockActive = true;
        }
    }

    //Режим отравляющего газа
    public void GasMode()
    {
        StopGas gasScript = ventilationGrid.GetComponent<StopGas>();
        if (!isGasActive)
        {
            gasScript.shoulWeStopIt = false;
            gasScript.GasIsOn();
            isGasActive = true;
        }
        else
        {
            gasScript.shoulWeStopIt = true;
            isGasActive = false;
        }
    }

    //Режим работы с ноутбуком
    public void LaptopMode()
    {
        if (!isLaptopModeActive)
        {
            oldPosition = laptop.transform.position;
            laptop.transform.SetParent(_player.GetComponent<Transform>());
            laptop.transform.SetPositionAndRotation(_player.transform.position, new Quaternion(0, 0, 0, 0));
            laptop.transform.Translate(0.05f, 0.3f, 0.05f);
            laptop.transform.Rotate(0, 180, 0);
            isLaptopModeActive = true;
        }
        else
        {
            Transform plotThing = GameObject.Find("PlotThings").GetComponent<Transform>();
            laptop.transform.SetParent(plotThing);
            laptop.transform.position = oldPosition;
            isLaptopModeActive = false;
        }
    }

    //Режим (де)активации телевизора
    public void TVMode()
    {
        if (!isTVActive)
        {
            //Активируем видеоплеер
            TV.transform.GetChild(1).gameObject.SetActive(true);
            //Вдавливаем кнопку
            TV.transform.GetChild(2).Translate(0, 0.02f, 0);
            isTVActive = true;
        }
        else
        {
            //Деактивируем видеоплеер
            TV.transform.GetChild(1).gameObject.SetActive(false);
            //Отпускаем кнопку
            TV.transform.GetChild(2).Translate(0, -0.02f, 0);
            isTVActive = false;
        }
    }

    void Update()
    {
        //Если игрок кликнул мышкой в режиме сбора предметов, запускаем проверку на столкновение с сюжетным предметом
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Смотрим, было ли столкновение луча с монтировкой (помечена тегом "Damage")
            if (Physics.Raycast(ray, out hit, 2) && hit.collider.tag == "Damage")
            {
                CrowbarMode();
            }
            //Смотрим, было ли столкновение луча с крышкой сейфа (помечена тегом "Cap")
            else if (Physics.Raycast(ray, out hit, 2) && hit.collider.tag == "Cap")
            {
                SeifLockMode();
            }
            //Смотрим, было ли столкновение луча с дверью
            else if (Physics.Raycast(ray, out hit, 5) && hit.collider.tag == "Finish")
            {
                DoorLockMode();
            }
            //Смотрим, было ли столкновение луча с ноутбуком
            else if (Physics.Raycast(ray, out hit, 14) && hit.collider.tag == "Laptop")
            {
                LaptopMode();
            }
            else if (Physics.Raycast(ray, out hit, 14) && hit.collider.name == "TVButton")
            {
               TVMode();
            }
        }

        //Переключаемся в режим сбора предметов нажатием на Q (если, конечно, мы не работаем с вводом текста на ноутбуке)
        if (Input.GetKeyDown(KeyCode.Q) && !isLaptopModeActive)
        {            
            ClickMode();
        }
    }
}
