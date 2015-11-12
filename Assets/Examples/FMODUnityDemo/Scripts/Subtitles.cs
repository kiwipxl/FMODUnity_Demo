using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using System.Runtime.InteropServices;

/*
* Handles subtitles.
* For example, in Studio, you can have a voice over sound 
* such as "This is a subtitle... Hello".
* You can then place a named marker above the audio called "This is a subtitle."
* and another one later on in the track called "Hello".
*
* You can use this class by attaching it to a UI Text component and then by
* calling the following:
* Subtitles.start(eventInstance);
*
* Any named markers found will change the text of the UI Text component
*/

public class Subtitles : MonoBehaviour
{
    private static Text subtitleText;
    private static EventInstance currentSubtitle = null;
    private static string targetSubtitleText = "";

    private void Start() {
        subtitleText = GetComponent<Text>();
        if (subtitleText == null) UnityEngine.Debug.LogError("No UI Text component added!");

        subtitleText.text = "";
    }

    private void Update()
    {
        if (currentSubtitle != null)
        {
            //if the current subtitle has stopped playing, then set the subtitle
            //text to nothing
            if (!isPlaying()) subtitleText.text = "";
        }

        subtitleText.text = targetSubtitleText;
    }

    public static void start(FMODAsset eventPath)
    {
        start(FMOD_StudioSystem.instance.GetEvent(eventPath));
    }

    public static void start(EventInstance eventInstance)
    {
        //stop the current subtitle playing if it exists
        if (currentSubtitle != null) currentSubtitle.stop(STOP_MODE.IMMEDIATE);

        //start the event instance
        eventInstance.start();

        /*
        ** Set a callback using the EventCallbackHelper used here for checking
        ** when a marker was received or when an event has stopped.
        **
        ** Note: The helper class is not part of the FMOD API or Unity integration, 
        ** it is just used for this demo.
        */
        EventCallbackHelper.setCallback(eventInstance, eventCallback);
        currentSubtitle = eventInstance;
    }

    public static void stop()
    {
        if (currentSubtitle != null) currentSubtitle.stop(STOP_MODE.IMMEDIATE);
    }

    private static void eventCallback(EventCallbackData data)
    {
        if (data.type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            //if a marker was encountered, then get the marker name and 
            //set it to the subtitle text
            targetSubtitleText = data.createMarker().name;
        }else if (data.type == EVENT_CALLBACK_TYPE.STOPPED)
        {
            //the subtitle event has ended, so set the subtitle text to blank
            targetSubtitleText = "";
        }
    }

    /*
    ** Returns whether or not the current subtitle is playing
    */
    public static bool isPlaying()
    {
        if (currentSubtitle == null) return false;

        PLAYBACK_STATE playState;
        currentSubtitle.getPlaybackState(out playState);

        return playState == PLAYBACK_STATE.PLAYING;
    }
}
