using UnityEngine;
using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using ImageEffects;

/*
* Handles pausing and unpausing the game (press escape or P to toggle).
* A pause snapshot instance and sound instance is created at runtime and 
* are played when paused.
*/

public class GamePause : MonoBehaviour
{
    public static bool isPaused = false;
    private GameObject pauseGameText;

    public EventRef pauseSnapshotPath;
    private EventInstance pauseSnapshot;
    public EventRef pauseSoundPath;
    private EventInstance pauseSound;

    private void Start()
    {
        //creates pause snapshot and sound event instances
        pauseSnapshot = RuntimeManager.CreateInstance(pauseSnapshotPath);
        pauseSound = RuntimeManager.CreateInstance(pauseSoundPath);

        pauseGameText = GameObject.Find("pauseGameText");

        unPauseGame();
    }

    private void Update()
    {
        pauseSound.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));

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
        isPaused = false;
        Time.timeScale = 1;

        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        pauseGameText.SetActive(false);

        pauseSound.stop(STOP_MODE.IMMEDIATE);
        pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
    }

    public void pauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;

        Camera.main.GetComponent<BlurOptimized>().enabled = true;
        pauseGameText.SetActive(true);

        pauseSound.start();
        pauseSnapshot.start();
    }
}
