using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private bool isMovingAble = false;
    private bool isPossibleToGoAround = false;
    void Start()
    {
        //Поворачиваем котика, чтобы ему было удобней спрыгнуть со стола
        //StartCoroutine(TurnCat(-90));
        transform.Translate(new Vector3(-0.8f,0,0));
        //Разрешаем котику двигаться в FixedUpdate
        isMovingAble = !isMovingAble;
    }

    IEnumerator TurnCat(int angle)
    {        
        //Hand - направление поворота (отрицательная либо положительная единица)
        int hand = Math.Abs(angle)/angle;
        Debug.Log($"Угол поворота: {hand}");
        //Получаем модуль угла, чтобы функция не зацикливалась, когда поворачиваемся влево
        angle = Math.Abs(angle);
        //Поворачиваемся на число градусов, указанное в условии
        while (angle > 0)
        {
            transform.Rotate(0, hand, 0);
            angle--;            
            yield return new WaitForSeconds(0.005f);
        }
        isMovingAble = true;
    }

    void FixedUpdate()
    {
        if (isMovingAble)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            Ray rayForward = new Ray(transform.position, transform.forward);
            Ray rayRight = new Ray(transform.position, transform.right);
            RaycastHit hit;

            if (Physics.SphereCast(rayForward, 0.1f, out hit))
            {
                if (hit.distance < 0.5)
                {
                    isMovingAble = false;
                    if (Physics.Raycast(rayRight, 0.5f))
                    {
                        StartCoroutine(TurnCat(-90));
                    }
                    else
                    {
                        StartCoroutine(TurnCat(90));
                    }
                }
            }
        }
    }
}
