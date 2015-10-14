using UnityEngine;
using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class Subtitles : MonoBehaviour {

    public EventRef p1WinsRef;
    EventInstance p1Wins;
    GUIText subtitleText;

	void Start() {
        p1Wins = RuntimeManager.CreateInstance(p1WinsRef);
        p1Wins.start();

        subtitleText = GetComponent<GUIText>();
        //subtitleText.text = "";

        EventCallback.setCallback(p1Wins, updateSubtitles, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    void updateSubtitles(EventCallbackData data)
    {
        if (data.type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            MarkerProperties marker = data.createMarker();
            subtitleText.text = marker.name;
            UnityEngine.Debug.Log(marker.name);
        }
    }
}
