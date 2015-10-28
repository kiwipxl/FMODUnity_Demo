using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

/*
* Creates a reverb 3D effect whose size depends on the sphere collider attached.
* There are many different presets, but 'HALLWAY' is used here.
*
* For more information, please see the following link:
* http://www.fmod.org/documentation/#content/generated/overview/3dreverb.htm
*/

public class ReverbEffect : MonoBehaviour
{
    public GameObject player;
    public GameObject playerTank;

    public EventRef reverbEventPath;
    private EventInstance reverbEvent;

    private float radius;

    private void Start()
    {
        radius = GetComponent<SphereCollider>().radius;

        reverbEvent = RuntimeManager.CreateInstance(reverbEventPath);
        reverbEvent.start();

        //creates a reverb3D instance from the low level system
        //Reverb3D reverb;
        //RuntimeManager.LowlevelSystem.createReverb3D(out reverb);

        ////sets reverb properties to a preset
        //REVERB_PROPERTIES props = PRESET.HALLWAY();
        //reverb.setProperties(ref props);

        ////sets the current position and min and max distances for the reverb 3D attributes
        //VECTOR pos = RuntimeUtils.ToFMODVector(transform.position);
        //float max_dist = radius * 2.0f;
        //float min_dist = max_dist / 2.0f;
        //reverb.set3DAttributes(ref pos, min_dist, max_dist);
	}

    private void Update()
    {
        float intensity;
        if (GameLogic.PerspectiveLogic.isPlayerRig) intensity = calcIntensity(player);
        else                                        intensity = calcIntensity(playerTank);

        reverbEvent.setParameterValue("Intensity", intensity);

        reverbEvent.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
    }

    private float calcIntensity(GameObject target)
    {
        //calculate distance, normalise it and then inverse it, so that
        //the closer the target, the higher the value (or intensity)
        float intensity = 1 - Vector3.Distance(transform.position, target.transform.position) / radius;

        //make sure intensity doesn't go below 0 or above 1.
        //if the target is far away from the radius, the value will be 0 (so the reverb won't be activated).
        intensity = Mathf.Clamp(intensity, 0.0f, 1.0f);

        return intensity;
    }
}
