using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Modes : MonoBehaviour
{
    bool isClickModeActive;
    GameObject _player;
    GameObject _camera;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        isClickModeActive = false;
    }
    public void ClickMode()
    {
        FPSInput moving = _player.GetComponent<FPSInput>();
        MouseLook rotationX = _player.GetComponent<MouseLook>();
        MouseLook rotationY = _camera.GetComponent<MouseLook>();
        

        if (isClickModeActive)
        {
            moving.movingAbility = 1;
            rotationX.rotationAbility = 1;
            rotationY.rotationAbility = 1;
            isClickModeActive = !isClickModeActive;
            Cursor.visible = false;
        }
        else
        {
            moving.movingAbility = 0;
            rotationX.rotationAbility = 0;
            rotationY.rotationAbility = 0;
            isClickModeActive = !isClickModeActive;
            Cursor.visible = true;
        }
        
    }

    public void CrowbarMode()
    {
        GameObject crowbar = GameObject.FindGameObjectWithTag("Damage");
        GameObject weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint");
        crowbar.transform.SetParent(_player.GetComponent<Transform>());
        crowbar.transform.position = weaponPoint.transform.position;
        crowbar.transform.rotation = weaponPoint.transform.rotation;
        //crowbar.transform.Rotate(-90, 0, -73);
    }

    public void Check()
    {
            Debug.Log("Клик сработал");
    }
}
