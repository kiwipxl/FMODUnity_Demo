using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles the audio for pausing and unpausing the game (press escape or P to toggle).
*
* This script is an example of a snapshot event, which is triggered when the
* game is paused.
*/

public class GamePauseAudio : MonoBehaviour
{
    //snapshot and sound asset (set in editor)
    [EventRef] public string pauseSnapshotPath;
    [EventRef] public string pauseSoundPath;

    //snapshot and sound instances
    private EventInstance pauseSnapshot;
    private EventInstance pauseSound;

    public void init() {
        //creates pause snapshot and sound event instances
        pauseSnapshot = RuntimeManager.CreateInstance(pauseSnapshotPath);
        pauseSound = RuntimeManager.CreateInstance(pauseSoundPath);
    }

    public void unPauseGame()
    {
        //stop snapshot and sound event
        pauseSound.stop(STOP_MODE.IMMEDIATE);
        pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
    }
    
    public void pauseGame()
    {
        //start snapshot and sound event
        pauseSound.start();
        pauseSnapshot.start();
    }
}
