using System.Collections.Generic;
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
    
    [SerializeField] public GameObject TV;
    [SerializeField] public GameObject cat;
    [SerializeField] private OnOffTV TVScript;
    [SerializeField] private GameObject laptop;
    [SerializeField] private GameObject ventilationGrid;
    [SerializeField] private GameObject seifLock = null;
    [SerializeField] private GameObject codeLock = null;
    [SerializeField] private GameObject plotThings = null;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private TurnOnAndOpenLaptop laptopControl;
    
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip doorSound;

    private GameObject _player = null;
    private GameObject _camera = null;
    private GameObject lockPoint;
    private Vector3 oldPosition;
    private Quaternion oldRotation;

    FPSInput playerMovingScript;
    MouseLook cameraRotationScript;
    
    private void Start()
    {
        //Ищем игрока через FIND, потому что он загружается из префаба (нельзя установить прямую ссылку)
        _player = GameObject.FindGameObjectWithTag("Player");
        playerMovingScript = _player.GetComponent<FPSInput>();
        // Получаем доступ к управляющему скрипту камеры
        _camera = Camera.main.gameObject;
        cameraRotationScript = _camera.GetComponent<MouseLook>();
        // Получаем доступ к точке, на которую будем "вешать" все головоломки
        lockPoint = cameraRotationScript.lockPoint;
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
        StopGas gasScript = ventilationGrid.GetComponent<StopGas>();
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
        if (!isLaptopModeActive)
        {
            oldPosition = laptop.transform.position;
            oldRotation = laptop.transform.rotation;
            laptop.transform.SetParent(_camera.transform);
            laptop.transform.position = lockPoint.transform.position;
            laptop.transform.rotation = lockPoint.transform.rotation;
            isLaptopModeActive = true;
            //Активируем кота: он запрыгивает на ящик, чтобы навести игрока на спасительную монтировку
            CatBehavior cB = cat.GetComponent<CatBehavior>();
            cB.isMovingPossible = false;
        }
        else
        {
            laptop.transform.SetParent(plotThings.transform);
            laptop.transform.position = oldPosition;
            laptop.transform.rotation = oldRotation;
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
