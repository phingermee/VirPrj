using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCheck : MonoBehaviour
{
    public static bool isGameLoad = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}