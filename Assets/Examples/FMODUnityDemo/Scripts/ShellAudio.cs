using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles all shell-related audio (shooting/explosions).
*
* The play functions are called from game logic scripts.
*/

public class ShellAudio : MonoBehaviour
{
    //shell event paths (set in Unity editor)
    public EventRef shellFirePath;
    public EventRef shellExplosionPath;

    private GameLogic.TankShooting tankShooting;    //tank shooting component

    private void Start()
    {
        tankShooting = GetComponent<GameLogic.TankShooting>();
    }

    public void playShellFire()
    {
        //play shell fire sound once and forget about it
        RuntimeManager.PlayOneShot(shellFirePath, Camera.main.transform.position);
    }

    public void playShellExplosion(Vector3 pos)
    {
        //creates a one time shell explosion sound and
        //sets it's position to where the shell exploded.
        EventInstance ev = RuntimeManager.CreateInstance(shellExplosionPath);
        ev.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
        ev.start();
        ev.release();
    }
}
