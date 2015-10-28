using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

/*
* Creates a reverb 3D effect whose size depends on the sphere collider attached.
*/

public class ReverbEffect : MonoBehaviour
{
    //reverb event path set in editor
    public EventRef reverbEventPath;
    private EventInstance reverbEvent;

    private void Start()
    {
        reverbEvent = RuntimeManager.CreateInstance(reverbEventPath);
        reverbEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        reverbEvent.start();
    }
}
