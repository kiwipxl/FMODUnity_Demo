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

    public void playShellExplosion(Vector3 pos, string layer)
    {
        //creates a one time shell explosion sound and sets the position 
        //to where the shell exploded.
        //also sets surface parameter based on what surface the shell hit

        int surfaceValue = 0;
        if (layer == "Water") surfaceValue = 1;
        else if (layer == "Building") surfaceValue = 2;

        EventInstance shellExplosion = FMOD_StudioSystem.instance.GetEvent(shellExplosionAsset);
        shellExplosion.setParameterValue("Surface", surfaceValue);
        shellExplosion.start();
        shellExplosion.release();
    }
}
