using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;

/*
* Handles subtitles
* todo: example of usage
* todo: how it works
*/

public class Subtitles : MonoBehaviour {

    public EventRef p1WinsRef;
    private EventInstance p1Wins;

    private static Text subtitleText;

	void Start() {
        subtitleText = GetComponent<Text>();
        subtitleText.text = "";

        p1Wins = RuntimeManager.CreateInstance(p1WinsRef);
        //startSubtitlesOn(p1Wins);
    }
    
    private static void updateSubtitles(EventCallbackData data)
    {
        if (data.type == EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {
            MarkerProperties marker = data.createMarker();

            ExecuteOnMainThread.queue.Enqueue(() =>
            {
                subtitleText.text = marker.name.ToUpper();
            });
        }
        else if (data.type == EVENT_CALLBACK_TYPE.STOPPED)
        {
            ExecuteOnMainThread.queue.Enqueue(() =>
            {
                subtitleText.text = "";
            });
        }
    }

    public static void startSubtitlesOn(EventInstance instance)
    {
        instance.start();
        EventCallback.setCallback(instance, updateSubtitles);
    }
}
