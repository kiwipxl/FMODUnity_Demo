using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;

/*
* Handles subtitles.
* For example, in Studio, you can have a voice over sound 
* such as "This is a subtitle. Hello".
* You can then place a named marker above the audio called "This is a subtitle."
* and another one later on in the track called "Hello".
*
* You can use this class by attaching it to a UI Text component and then by
* calling the following:
* Subtitles.start(eventInstance);
*
* Any named markers found will change the text of the UI Text component
*/

public class Subtitles : MonoBehaviour {

    public EventRef[] subtitleEventList;

    private static Text subtitleText;
    private static EventInstance currentSubtitle = null;

    private void Start() {
        subtitleText = GetComponent<Text>();
        if (subtitleText == null) UnityEngine.Debug.LogError("No UI Text component added!");

        subtitleText.text = "";
    }

    private void Update()
    {
        //temporary code
        //press B to spawn random subtitles from a list set in the editor
        if (Input.GetKeyDown(KeyCode.B))
        {
            start(subtitleEventList[UnityEngine.Random.Range(0, subtitleEventList.Length)]);
        }

        if (currentSubtitle != null)
        {
            //set the subtitle event sound position to the camera position (where the listener is) to fake a 2D sound
            currentSubtitle.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));

            //check if the current subtitle has ended
            PLAYBACK_STATE playbackState;
            currentSubtitle.getPlaybackState(out playbackState);
            if (playbackState == PLAYBACK_STATE.STOPPED) subtitleText.text = "";
        }
    }

    private static void eventCallback(EventCallbackData data)
    {
        if (data.type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            //if a marker has been hit, then create the marker properties
            //data and set the subtitles text to the name of the marker

            MarkerProperties marker = data.createMarker();

            subtitleText.text = marker.name.ToUpper();
        }
    }

    public static void start(EventInstance eventInstance)
    {
        //stop the current subtitle playing if it exists
        if (currentSubtitle != null) currentSubtitle.stop(STOP_MODE.IMMEDIATE);

        //start the event instance and release
        eventInstance.start();
        eventInstance.release();

        //set the callback
        EventCallback.setCallback(eventInstance, eventCallback);
        currentSubtitle = eventInstance;
    }

    public static void start(EventRef eventPath)
    {
        start(RuntimeManager.CreateInstance(eventPath));
    }
}
