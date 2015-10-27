using UnityEngine;
using FMOD;
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
    private void Start()
    {
        float radius = GetComponent<SphereCollider>().radius;

        //creates a reverb3D instance from the low level system
        Reverb3D reverb;
        RuntimeManager.LowlevelSystem.createReverb3D(out reverb);

        //sets reverb properties to a preset
        REVERB_PROPERTIES props = PRESET.HALLWAY();
        reverb.setProperties(ref props);

        //sets the current position and min and max distances for the reverb 3D attributes
        VECTOR pos = RuntimeUtils.ToFMODVector(transform.position);
        float max_dist = radius * 2.0f;
        float min_dist = max_dist / 2.0f;
        reverb.set3DAttributes(ref pos, min_dist, max_dist);
	}
}
