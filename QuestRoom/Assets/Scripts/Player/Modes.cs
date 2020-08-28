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
    
    //Режим сбора предметов (игрок "заморожен", курсор активен)
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

    //Режим монтировки (монтировка в руках, можно разрушить окно)
    public void CrowbarMode()
    {
        GameObject crowbar = GameObject.FindGameObjectWithTag("Damage");
        GameObject weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint");
        crowbar.transform.SetParent(_player.GetComponent<Transform>());
        crowbar.transform.position = weaponPoint.transform.position;
        crowbar.transform.rotation = weaponPoint.transform.rotation;
    }

    //Режим громкой улицы (когда окно разбито)
    public void LoudStreetMode()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        float musicVolume = 0.5f;
        audioSrc.volume = musicVolume;
    }

    public void Check()
    {
            Debug.Log("Клик сработал");
    }
}
