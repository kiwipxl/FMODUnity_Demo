using UnityEngine;
using FMODUnity;

/*
** Plays a reverb 3D distance effect event.
*/

public class ReverbEffect : MonoBehaviour
{
    //reverb event asset set in editor
    [EventRef] public string reverbEventPath;

    private void Start()
    {
        //play reverb event asset at the reverb object position
        RuntimeManager.PlayOneShot(reverbEventPath, transform.position);
    }
}
