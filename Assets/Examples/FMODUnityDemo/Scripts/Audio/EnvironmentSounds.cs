using UnityEngine;
using FMOD.Studio;

/*
* Plays an ambient environmental event that changes according to the time of the day.
*/

public class EnvironmentSounds : MonoBehaviour
{
    //environment asset event (set in editor)
    public FMODAsset environmentAsset;

    private EventInstance environmentEvent;     //environment event instance

    private void Start()
    {
        //get event and start playing it
        environmentEvent = FMOD_StudioSystem.instance.GetEvent(environmentAsset);
        environmentEvent.start();
    }

    private void Update()
    {
        //update the time of day parameter on the environment event
        environmentEvent.setParameterValue("TimeOfDay", GameLogic.Environment.hours);
	}
}
