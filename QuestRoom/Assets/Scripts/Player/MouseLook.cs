using UnityEngine;
using System.Collections;
public class MouseLook : MonoBehaviour
{
    //Шифруем направления движения муши, чтобы получать к ним доступ через переменные
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    //Ориентация мыши по умолчанию
    public RotationAxes axes = RotationAxes.MouseXAndY;
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
        Rigidbody body = GetComponent<Rigidbody>();
        //Отключаем влияние среды на игрока (шоб не кулялся и не отскакивал к звёздам)
        if (body != null)
            body.freezeRotation = true;
    }
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor * rotationAbility, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            //Увеличиваем угол поворота по вертикали в соответствии с перемещениями указателя  мыши
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * rotationAbility;
            //Фиксируем угол поворота по вертикали в диапазоне, заданном минимальным и максимальным значениями. 
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            //Сохраняем одинаковый угол поворота вокруг оси Y (т. е. вращение в горизонтальной плоскости отсутствует)
            float rotationY = transform.localEulerAngles.y;
            //Создаем  новый вектор из сохраненных значений поворота
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
        else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert * rotationAbility;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            //Приращение угла поворота через значение delta. 
            float delta = Input.GetAxis("Mouse X") * sensitivityHor * rotationAbility;
            //Значение delta — это величина изменения угла поворота
            float rotationY = transform.localEulerAngles.y + delta;
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }
}