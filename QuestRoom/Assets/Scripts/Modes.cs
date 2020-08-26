using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modes : MonoBehaviour
{
    bool isClickModeActive;
    public void ClickMode()
    {        
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        GameObject _camera = GameObject.FindGameObjectWithTag("MainCamera");
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
}
