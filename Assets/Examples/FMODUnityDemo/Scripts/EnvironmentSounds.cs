using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Plays an ambient environmental event that changes according to the time of the day.
*/

public class EnvironmentSounds : MonoBehaviour
{
    public EventRef envEventPath;   //event path set in unity editor

    private EventInstance envEvent; //environment event instance

    private void Start()
    {
        //create instance and start playing it
        envEvent = RuntimeManager.CreateInstance(envEventPath);
        envEvent.start();
    }

    private void Update()
    {
        //update the time of day parameter on the environment event
        envEvent.setParameterValue("TimeOfDay", GameLogic.Environment.hours);
	}
}
