using UnityEngine;
using FMOD.Studio;

/*
* Handles all shell-related audio (shooting/explosions).
*
* The play audio functions are called from game logic scripts.
*/

public class ShellAudio : MonoBehaviour
{
    //shell event assets (set in Unity editor)
    public FMODAsset shellFireAsset;
    public FMODAsset shellExplosionAsset;

    public void playShellFire()
    {
        //play shell fire sound once on the tank and then forget about it
        FMOD_StudioSystem.instance.PlayOneShot(shellFireAsset, transform.position);
    }

    public void playShellExplosion(Vector3 pos)
    {
        //creates a one time shell explosion sound and sets the position 
        //to where the shell exploded.
        FMOD_StudioSystem.instance.PlayOneShot(shellExplosionAsset, pos);
    }
}
