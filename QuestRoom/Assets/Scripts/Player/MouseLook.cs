using UnityEngine;

//   Скрипт описывает поведение камеры
public class MouseLook : MonoBehaviour
{
    //Вводим в(ы)ключатель вращения (умножаем на него скорость всех вращений)
    public float rotationAbility = 1.0f;
    public GameObject lockPoint = null;

    [SerializeField] private GameObject _player = null;

    //Чувствительность мыши
    private float sensitivityHor = 9.0f;
    private float sensitivityVert = 9.0f;
    //Ограничиваем обзор по вертикали
    private float minimumVert = -45.0f;
    private float maximumVert = 45.0f;
    //Объявляем закрытую переменную для угла поворота по вертикали
    private float _rotationX = 0;
    
    void Start()
    {
        Rigidbody body = _player.GetComponent<Rigidbody>();
        //Отключаем влияние среды на игрока (шоб не кулялся и не отскакивал к звёздам)
        if (body != null)
            body.freezeRotation = true;
    }

    //Управляем вращением камеры
    void rotationCam()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * rotationAbility;
        _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
        //Приращение угла поворота через значение delta. 
        float delta = Input.GetAxis("Mouse X") * sensitivityHor * rotationAbility;
        //Значение delta — это величина изменения угла поворота
        float rotationY = _player.transform.localEulerAngles.y + delta;
        //Вращение вверх-вниз разрешаем только камере, чтобы гавитация не утаскивала персонажа вперёд-назад
        _player.transform.localEulerAngles = new Vector3(0, rotationY, 0);
        transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    void Update()
    {
        //Управляем вращением камеры
        rotationCam();
    }
}