using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Plays an ambient environmental event that changes according to the time of the day.
*/

public class EnvironmentSounds : MonoBehaviour
{
    //environment asset event (set in editor)
    [EventRef] public string environmentPath;

    private EventInstance environmentEvent;     //environment event instance

    private void Start()
    {
        //get event and start playing it
        environmentEvent = RuntimeManager.CreateInstance(environmentPath);
        environmentEvent.start();
    }

    private void Update()
    {
        //update the time of day parameter on the environment event
        environmentEvent.setParameterValue("TimeOfDay", GameLogic.Environment.hours);
	}
}
