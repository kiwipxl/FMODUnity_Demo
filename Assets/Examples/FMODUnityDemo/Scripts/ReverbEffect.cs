using UnityEngine;
using FMOD;
using FMOD.Studio;

/*
* Creates a reverb 3D effect whose size depends on the sphere collider attached.
*/

public class ReverbEffect : MonoBehaviour
{
    //reverb event asset set in editor
    public FMODAsset reverbEventAsset;
    private EventInstance reverbEvent;

    private void Start()
    {
        reverbEvent = FMOD_StudioSystem.instance.GetEvent(reverbEventAsset);
        reverbEvent.set3DAttributes(UnityUtil.to3DAttributes(transform.position));
        reverbEvent.start();
    }
}
