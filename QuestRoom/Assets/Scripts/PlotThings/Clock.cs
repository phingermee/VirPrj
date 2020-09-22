using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//
//  Simple Clock Script / Andre "AEG" Bürger / VIS-Games 2012
//
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------

    //-- set start time 00:00
    public int minutes = 0;
    public int hour = 0;
    
    //-- time speed factor
   // public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster

    //-- internal vars
    int seconds;
    float msecs;
    private GameObject pointerSeconds;
    private GameObject pointerMinutes;
    private GameObject pointerHours;
    [SerializeField] private DayNight dayNightScript;
    [SerializeField] private GameObject laptopPanel;
    private Text laptopClock;
    private float daySpeed;
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        daySpeed = dayNightScript.daySpeed;
        pointerSeconds = transform.Find("rotation_axis_pointer_seconds").gameObject;
        pointerMinutes = transform.Find("rotation_axis_pointer_minutes").gameObject;
        pointerHours = transform.Find("rotation_axis_pointer_hour").gameObject;

        msecs = 0.0f;
        seconds = 0;

        laptopClock = laptopPanel.GetComponent<Text>();
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        hour = 12;
        //-- calculate time
        msecs += Time.deltaTime * daySpeed;
        if (msecs >= 1.0f)
        {
            msecs -= 1.0f;
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
                if (minutes > 60)
                {
                    minutes = 0;
                    hour++;
                    if (hour >= 24)
                        hour = 0;
                }
            }
        }


        //-- calculate pointer angles
        float rotationSeconds = (360.0f / 60.0f) * seconds;
        float rotationMinutes = (360.0f / 60.0f) * minutes;
        float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

        //-- draw pointers
        pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
        pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
        pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);

        if (laptopPanel.activeSelf)
        {
            if (minutes / 10 >= 1)
            {
                laptopClock.text = $"{hour}h:{minutes}min";
            }
            else
            {
                laptopClock.text = $"{hour}h:0{minutes}min";
            }
        }
    }
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------
}
