using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public float dayDuration = 30f;
    public GameObject target;
    public Light sun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.RotateAround(target.transform.position, new Vector3(1, 0, 0), 1/dayDuration);
    }
}
