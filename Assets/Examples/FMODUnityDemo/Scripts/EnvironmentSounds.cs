using UnityEngine;
using FMOD.Studio;

/*
* Plays an ambient environmental event that changes according to the time of the day.
*/

public class EnvironmentSounds : MonoBehaviour
{
    public FMODAsset environmentAsset;

    private EventInstance environmentEvent;     //environment event instance

    private void Start()
    {
        //create instance and start playing it
        environmentEvent = FMOD_StudioSystem.instance.GetEvent(environmentAsset);
        environmentEvent.start();
    }

    private void Update()
    {
        //update the time of day parameter on the environment event
        environmentEvent.setParameterValue("TimeOfDay", GameLogic.Environment.hours);
	}
}
