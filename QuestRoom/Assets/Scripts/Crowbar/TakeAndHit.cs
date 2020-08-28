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
        //Если игрок кликнул на монтировку в режиме сбора предметов, включается Режим Монтировки (позволяет разбить окно)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Damage")
            {
                mode.CrowbarMode();
            }
        }
    }
}
