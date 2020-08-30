using UnityEngine;
using System.Collections;
public class MouseLook : MonoBehaviour
{
    GameObject _player;
    //Чувствительность мыши
    float sensitivityHor = 9.0f;
    float sensitivityVert = 9.0f;
    //Ограничиваем обзор по вертикали
    float minimumVert = -45.0f;
    float maximumVert = 45.0f;
    //Объявляем закрытую переменную для угла поворота по вертикали
    private float _rotationX = 0;
    //Вводим выключатель (умножаем на него скорость всех перемещений)
    public float rotationAbility = 1.0f;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody body = _player.GetComponent<Rigidbody>();
        //Отключаем влияние среды на игрока (шоб не кулялся и не отскакивал к звёздам)
        if (body != null)
            body.freezeRotation = true;
    }
    void Update()
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
}