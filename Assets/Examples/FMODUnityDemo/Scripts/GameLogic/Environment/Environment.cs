using UnityEngine;
using UnityEngine.UI;

/*
* Handles the environment time logic.
*/

namespace GameLogic
{
    public class Environment : MonoBehaviour
    {
        //light object and day/night colours (set in editor)
        public Light envLight;
        public Color dayColour;
        public Color nightColour;

        private const float TRANSITION_TIME = 2.0f; //the amount of hours it takes to transition from day/night
        private float dayHourStart = 6.0f;          //the hour (24 hour time) the day time transition starts
        private float nightHourStart = 18.0f;       //the hour (24 hour time) the night time transition starts

        private Text timeText;      //the UI text component
        public static int hours;          //current hour (24 hour time)
        private float minutes;      //current minutes

        private const float MINS_PER_SECOND = 10;           //minutes per second that the time moves
        private const float MINS_PER_SECOND_SPEEDUP = 250;  //when a speedup key is pressed, use this value per second instead

        private void Start()
        {
            //get time text UI object
            timeText = GameObject.Find("timeText").GetComponent<Text>();

            //set initial time to current machine time
            System.DateTime time = System.DateTime.Now;
            minutes = time.Minute;
            hours = time.Hour;
        }

        private void Update()
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
            if (hoursF >= dayHourStart - TRANSITION_TIME && hoursF <= nightHourStart - TRANSITION_TIME)
            {
                timeNormalised = 0.0f;
                if (hoursF >= dayHourStart - TRANSITION_TIME && hoursF <= dayHourStart + TRANSITION_TIME)
                {
                    timeNormalised = 1 - (((hoursF - dayHourStart) / (TRANSITION_TIME * 2.0f))) - .5f;
                }
            }
            else
            {
                timeNormalised = 1.0f;
                if (hoursF >= nightHourStart - TRANSITION_TIME && hoursF <= nightHourStart + TRANSITION_TIME)
                {
                    timeNormalised = ((hoursF - nightHourStart) / (TRANSITION_TIME * 2.0f)) + .5f;
                }
            }

            //lerp day colour and night colour based on normalised time
            Color envColour = Color.Lerp(dayColour, nightColour, timeNormalised);
            envLight.color = envColour;
        }
    }
};
