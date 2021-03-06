﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhotonTutorial.Movement;

/*
   Самый длинный скрипт, описывающий различные режимы игры. Если точнее, он описывает начальный этап
   работы с сюжетными предметами: отсюда запускаются их скрипты - начало генерации газового облака, включение телевизора,
   перемещение ноутбука со стола в руки игрока, "запуск" движения котика, усиление громкости улицы при разбитом окне и так далее
 */
public class Modes : MonoBehaviour
{
    public bool isGasActive = false;
    public bool isSeifOpen = false;
    public bool isDoorOpen = false;
    public bool iscodeLockActive = false;
    public bool _isBoxLockActive = false;
    public bool isLaptopModeActive = false;
    public bool isLaptotOn = false;
    public bool isTVActive = false;
    public bool isClickModeActive = false;

    public List<bool> questTasks = new List<bool>(3) { false, false, false };

    [SerializeField] public Animator seifCapAnim;
    [SerializeField] public Animator openDoorAnim;
    [SerializeField] public Animator TVButtonAnim;
    [SerializeField] public Animator LaptopButtonAnim;
    [SerializeField] public GameObject TV = null;
    [SerializeField] public GameObject cat = null;
    [SerializeField] public GameObject seifCap = null;
    [SerializeField] public GameObject door = null;
    [SerializeField] private OnOffTV TVScript = null;
    [SerializeField] private GameObject laptop = null;
    [SerializeField] private GameObject seifLock = null;
    [SerializeField] private GameObject codeLock = null;    
    [SerializeField] private GameObject plotThings = null;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private TurnOnAndOpenLaptop laptopControl;
    [SerializeField] private StopGas gasScript = null;
    
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip doorSound;

    private GameObject _player = null;
    private GameObject _camera = null;
    private Transform lockPoint;
    private Vector3 oldPosition;
    private Quaternion oldRotation;

    private FPSInput playerMovingScript;
    private MouseLook cameraRotationScript;
    
    private void Start()
    {
        //Ищем игрока через FIND, потому что он загружается из префаба (нельзя установить прямую ссылку)
        _player = GameObject.FindGameObjectWithTag("Player");
        // Получаем доступ к скрипту, который управляет движением игрока
        playerMovingScript = _player.GetComponent<FPSInput>();
        // Получаем доступ к управляющему скрипту камеры
        _camera = Camera.main.gameObject;
        cameraRotationScript = _camera.GetComponent<MouseLook>();
        // Получаем доступ к точке, на которую будем "вешать" все головоломки
        lockPoint = cameraRotationScript.lockPoint.transform;
    }

    //Режим сбора предметов: "замораживаем" игрока и активируем курсор (и наоборот)
    public void ClickMode()
    {
        isClickModeActive = !isClickModeActive;
        playerMovingScript.movingAbility = isClickModeActive ? 0 : 1;
        cameraRotationScript.rotationAbility = isClickModeActive ? 0 : 1;
        Cursor.visible = isClickModeActive;
    }

    //Режим громкой улицы (когда окно разбито)
    public void LoudStreetMode()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = 0.5f;
    }

    //Режим работы с кодовым замком на ящике (сейфе)
    public void SeifLockMode()
    {
        //Если сейф до сих пор не открыт, подгружаем головоломку из префаба и "вешаем" её на игрока
        if (!_isBoxLockActive && !isSeifOpen)
        {
            Instantiate(seifLock, _camera.transform.position, Quaternion.identity);
            _isBoxLockActive = true;
        }
    }

    //Режим работы с кодовым замком на двери
    public void DoorLockMode()
    {
        //Считаем, сколько заданий выполнено
        var finishCheckingCount = questTasks.Count(n => n);
        //Если выполнены не все задания, то...
        if (finishCheckingCount < 3)
        {
            audio.volume = 1;
            audio.PlayOneShot(doorSound);
        }
        //Если все задания выполнены, но дверь до сих пор не открыта, подгружаем головоломку из префаба и "вешаем" её на игрока
        else if (!iscodeLockActive && !isDoorOpen)
        {
            Instantiate(codeLock, _camera.transform.position, Quaternion.identity);
            iscodeLockActive = true;
        }
    }

    //Режим отравляющего газа
    public void GasMode()
    {
        gasScript.shouldWeStopIt = isGasActive;
        if (!isGasActive)
        {
            gasScript.GasIsOn();
        }
        isGasActive = !isGasActive;
    }

    //Режим работы с ноутбуком
    public void LaptopMode()
    {
        //Если ноут ещё не активирован (стоит на столе), то берём его в руки и поворачиваем экраном к игроку
        if (!isLaptopModeActive)
        {
            oldPosition = laptop.transform.position;
            oldRotation = laptop.transform.rotation;
            laptop.transform.SetParent(_camera.transform);
            //"Вешаем" ноутбук на координаты пустого объекта, закреплённого перед камерой
            laptop.transform.SetPositionAndRotation(lockPoint.position, lockPoint.rotation);
            isLaptopModeActive = true;
            //Активируем кота: он запрыгивает на ящик, чтобы навести игрока на спасительную монтировку
            CatBehavior cB = cat.GetComponent<CatBehavior>();
            cB.isMovingPossible = false;
        }
        //Если мы работаем с ноутом, то при нажатии ТАВ возвращаем ноут на старое место
        else
        {
            laptop.transform.SetParent(plotThings.transform);
            laptop.transform.SetPositionAndRotation(oldPosition, oldRotation);
            isLaptopModeActive = false;
        }
    }

    // Включение/выключение ноутбука, когда он уже в руках
    public void LaptopOn() => laptopControl.TurnOnOff();

    // Открытие сюжетного файла по двойному клику
    public void TextFileClick() => laptopControl.IconClick();

    //Режим (де)активации телевизора
    public void TVMode()
    {
        TVScript.TVturnOnOff(isTVActive);
        isTVActive = !isTVActive;
    }

    //Режим (де)активации меню паузы
    public void PauseMenuMode()
    {
        if (!pauseMenuPanel.activeSelf)
        {
            isClickModeActive = false;
            ClickMode();
            pauseMenuPanel.SetActive(true);
        }
        else pauseMenuPanel.SetActive(false);
    }
}
