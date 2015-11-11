using UnityEngine;
using FMOD.Studio;

/*
* Handles all shell-related audio (shooting/explosions).
*
* The play functions are called from game logic scripts.
*/

public class ShellAudio : MonoBehaviour
{
    //shell event assets (set in Unity editor)
    public FMODAsset shellFireAsset;
    public FMODAsset shellExplosionAsset;

    private GameLogic.TankShooting tankShooting;    //tank shooting component

    private void Start()
    {
        tankShooting = GetComponent<GameLogic.TankShooting>();
    }

    public void playShellFire()
    {
        //play shell fire sound once on the tank and then forget about it
        FMOD_StudioSystem.instance.PlayOneShot(shellFireAsset, transform.position);
    }

    public void playShellExplosion(Vector3 pos)
    {
        //creates a one time shell explosion sound and
        //sets it's position to where the shell exploded.
        EventInstance ev = FMOD_StudioSystem.instance.GetEvent(shellExplosionAsset);
        ev.set3DAttributes(UnityUtil.to3DAttributes(pos));
        ev.start();
        ev.release();
    }
}
