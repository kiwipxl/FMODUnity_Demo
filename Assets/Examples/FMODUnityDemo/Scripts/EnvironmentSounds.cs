using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using FMODUnity;

public class EnvironmentSounds : MonoBehaviour
{
    public EventRef envEventPath;
    private EventInstance envEvent;

    public Light envLight;
    public Color dayColour;
    public Color nightColour;

    private const float DAY_SWITCH = 1.0f;
    private float dayHourStart = 6.0f;
    private float nightHourStart = 18.0f;

    private Text timeText;
    private int hours;
    private float minutes;
    private const float MINS_PER_SECOND = 10;
    private const float MINS_PER_SECOND_SPEEDUP = 100;

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
    }

    private void updateTime()
    {
        if (GamePause.isPaused) return;

        //calculate game time
        if (Input.GetKey(KeyCode.O)) minutes += MINS_PER_SECOND_SPEEDUP * Time.deltaTime;
        else minutes += MINS_PER_SECOND * Time.deltaTime;

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

        //calculate hours with floats instead of whole numbers.
        float hoursF = hours + (minutes / 60.0f);

        //calculate normalised time for lerping colours
        float timeNormalised;
        if (hoursF >= dayHourStart - DAY_SWITCH && hoursF <= nightHourStart - DAY_SWITCH)
        {
            timeNormalised = 0.0f;
            if (hoursF >= dayHourStart - DAY_SWITCH && hoursF <= dayHourStart + DAY_SWITCH)
            {
                timeNormalised = 1 - (((hoursF - dayHourStart) / (DAY_SWITCH * 2.0f)) + (DAY_SWITCH / 2.0f));
            }
        }else {
            timeNormalised = 1.0f;
            if (hoursF >= nightHourStart - DAY_SWITCH && hoursF <= nightHourStart + DAY_SWITCH)
            {
                timeNormalised = ((hoursF - nightHourStart) / (DAY_SWITCH * 2.0f)) + (DAY_SWITCH / 2.0f);
            }
        }

        //lerp day colour and night colour based on normalised time
        Color envColour = Color.Lerp(dayColour, nightColour, timeNormalised);
        envLight.color = envColour;
    }
}
