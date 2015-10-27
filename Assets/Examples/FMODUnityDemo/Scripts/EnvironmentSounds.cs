using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class EnvironmentSounds : MonoBehaviour
{
    public EventRef envEventPath;
    private EventInstance envEvent;

    private Text timeText;
    private int hours;
    private float minutes;
    private const float MINS_PER_SECOND = 100;

    private void Start()
    {
        envEvent = RuntimeManager.CreateInstance(envEventPath);
        envEvent.start();

        initTime();
    }

    private void Update()
    {
        updateTime();

        envEvent.setParameterValue("TimeOfDay", hours);
	}

    /*
    * Below is the game time logic code.
    */

    private void initTime()
    {
        //get time text UI object
        timeText = GameObject.Find("timeText").GetComponent<Text>();

        //set initial time to current machine time
        System.DateTime time = System.DateTime.Now;
        minutes = time.Minute;
        hours = time.Hour;
        hours = 8;
    }

    private void updateTime()
    {
        if (GamePause.isPaused) return;

        //calculate game time
        minutes += MINS_PER_SECOND * Time.deltaTime;
        if (minutes >= 60)
        {
            minutes = 0;
            ++hours;
            if (hours > 24)
            {
                hours = 1;
            }
        }

        //format time and display
        bool is_am = hours < 12;
        if (hours == 24) is_am = true;
        int hourWrapped = hours <= 12 ? hours : hours - 12;

        timeText.text = (hourWrapped < 10 ? "0" : "") + hourWrapped + ":" +
                        ((int)minutes < 10 ? "0" : "") + (int)minutes +
                        (is_am ? "am" : "pm");
    }
}
