using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using ImageEffects;

/*
* Handles pausing and unpausing the game (press escape or P to toggle).
*
* This script is an example of a snapshot event, which is triggered when the
* game is paused.
*/

public class GamePause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseScreenGroup;

    public GameObject playerHuman;
    private Listener humanListener;

    //snapshot and sound path (set in editor)
    public EventRef pauseSnapshotPath;
    public EventRef pauseSoundPath;

    //snapshot and sound instances
    private EventInstance pauseSnapshot;
    private EventInstance pauseSound;

    private void Start()
    {
        //creates pause snapshot and sound event instances
        pauseSnapshot = RuntimeManager.CreateInstance(pauseSnapshotPath);
        pauseSound = RuntimeManager.CreateInstance(pauseSoundPath);

        unPauseGame();
    }

    private void Update()
    {
        if (isPaused && Input.GetMouseButtonUp(0)) unPauseGame();
        if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) togglePause();
    }

    public void togglePause()
    {
        if (isPaused) unPauseGame();
        else pauseGame();
    }

    public void unPauseGame()
    {
        //set time to 1
        isPaused = false;
        Time.timeScale = 1;

        //disable blur on camera and "Game Paused" text
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        pauseScreenGroup.SetActive(false);

        //stop snapshot and sound event
        pauseSound.stop(STOP_MODE.IMMEDIATE);
        pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
    }

    public void pauseGame()
    {
        //set time to 0
        isPaused = true;
        Time.timeScale = 0;

        //enable blur on camera and "Game Paused" text
        Camera.main.GetComponent<BlurOptimized>().enabled = true;
        pauseScreenGroup.SetActive(true);

        //start snapshot and sound event
        pauseSound.start();
        pauseSnapshot.start();
    }
}
