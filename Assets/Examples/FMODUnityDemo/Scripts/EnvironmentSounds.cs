using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class EnvironmentSounds : MonoBehaviour
{
    public EventRef envEventPath;
    private EventInstance envEvent;

    public GameObject envObj;
    private GameLogic.Environment env;

    private void Start()
    {
        envEvent = RuntimeManager.CreateInstance(envEventPath);
        envEvent.start();

        env = envObj.GetComponent<GameLogic.Environment>();
        if (env == null) Debug.LogError("Could not find 'Environment' component");
        env.initTime();
    }

    private void Update()
    {
        env.updateTime();

        envEvent.setParameterValue("TimeOfDay", env.getHours());
	}
}
