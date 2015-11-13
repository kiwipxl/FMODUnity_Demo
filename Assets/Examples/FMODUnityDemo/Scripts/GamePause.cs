using UnityEngine;
using FMOD.Studio;

/*
* Handles the audio for pausing and unpausing the game (press escape or P to toggle).
*
* This script is an example of a snapshot event, which is triggered when the
* game is paused.
*/

public class GamePause : MonoBehaviour
{
    //snapshot and sound asset (set in editor)
    public FMODAsset pauseSnapshotAsset;
    public FMODAsset pauseSoundAsset;

    //snapshot and sound instances
    private EventInstance pauseSnapshot;
    private EventInstance pauseSound;

    public static bool isPaused = false;

    public void init() {
        //creates pause snapshot and sound event instances
        pauseSnapshot = FMOD_StudioSystem.instance.GetEvent(pauseSnapshotAsset);
        pauseSound = FMOD_StudioSystem.instance.GetEvent(pauseSoundAsset);
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
