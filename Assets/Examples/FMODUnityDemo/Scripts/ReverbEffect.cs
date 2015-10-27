using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class ReverbEffect : MonoBehaviour
{
    private void Start()
    {
        float radius = GetComponent<SphereCollider>().radius;

        Reverb3D reverb;
        RuntimeManager.LowlevelSystem.createReverb3D(out reverb);
        REVERB_PROPERTIES props = PRESET.HALLWAY();
        reverb.setProperties(ref props);

        VECTOR pos = RuntimeUtils.ToFMODVector(transform.position);
        float max_dist = radius * 2.0f;
        float min_dist = max_dist / 2.0f;
        reverb.set3DAttributes(ref pos, min_dist, max_dist);
	}
}
