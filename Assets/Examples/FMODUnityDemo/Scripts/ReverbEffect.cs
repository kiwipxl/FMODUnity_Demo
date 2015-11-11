using UnityEngine;
using FMOD;
using FMOD.Studio;

/*
** Plays a reverb 3D distance effect event.
*/

public class ReverbEffect : MonoBehaviour
{
    //reverb event asset set in editor
    public FMODAsset reverbEventAsset;

    private void Start()
    {
        //play reverb event asset at the reverb object position
        FMOD_StudioSystem.instance.PlayOneShot(reverbEventAsset, transform.position);
    }
}
