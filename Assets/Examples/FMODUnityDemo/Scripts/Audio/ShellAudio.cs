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

    /* Called when a shell has been fired from the tank */
    public void playShellFire()
    {
        //play shell fire sound once on the tank and then forget about it.
        FMOD_StudioSystem.instance.PlayOneShot(shellFireAsset, transform.position);
    }

    /* Called when a shell has exploded */
    public void playShellExplosion(Vector3 pos, string layer)
    {
        // Sets surface parameter based on what surface the shell hit.
        int surfaceValue = 0;
        if (layer == "Water") surfaceValue = 1;
        else if (layer == "Building") surfaceValue = 2;

        // Creates a one time shell explosion sound and sets the position 
        // to where the shell exploded.
        EventInstance shellExplosion = FMOD_StudioSystem.instance.GetEvent(shellExplosionAsset);
        shellExplosion.setParameterValue("Surface", surfaceValue);
        shellExplosion.start();
        shellExplosion.release();
    }
}
