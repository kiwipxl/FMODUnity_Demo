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

    private GameLogic.TankShooting tankShooting;

    private void Start()
    {
        tankShooting = GetComponent<GameLogic.TankShooting>();
    }

    public void playShellFire()
    {
        //play shell fire sound once and forget about it
        RuntimeManager.PlayOneShot(shellFirePath, Camera.main.transform.position);
    }

    public void playShellExplosion(float volume)
    {
        //creates a shell explosion instance and sets it's position to the camera (where the
        //listener is) to fake a 2D sound
        EventInstance ev = RuntimeManager.CreateInstance(shellExplosionPath);
        ev.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
        ev.setVolume(volume);
        ev.start();
        ev.release();
    }
}
