using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles all shell-related audio (shooting/explosions)
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
        RuntimeManager.PlayOneShot(shellFirePath, Camera.main.transform.position);
    }

    public void playShellExplosion(float volume)
    {
        EventInstance ev = RuntimeManager.CreateInstance(shellExplosionPath);
        ev.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
        ev.setVolume(volume);
        ev.start();
        ev.release();
    }
}
