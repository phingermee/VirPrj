using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakeAndHit : MonoBehaviour
{
    //Получаем доступ к списку режимов игры
    Modes mode;
    Camera _camera;

    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();
    }

    void Update()
    {
        //Если игрок кликнул на сюжетный объект в режиме сбора предметов, включается соответствубщий режим
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Смотрим, было ли столкновение луча с монтировкой (помечена тегом "Damage")
            if (Physics.Raycast(ray, out hit, 2) && hit.collider.tag == "Damage")
            {
                mode.CrowbarMode();
            }
            //Смотрим, было ли столкновение луча с крышкой сейфа (помечена тегом "Cap")
            else if (Physics.Raycast(ray, out hit, 2) && hit.collider.tag == "Cap")
            {
                mode.SeifLockMode();
            }
        }
    }
}
