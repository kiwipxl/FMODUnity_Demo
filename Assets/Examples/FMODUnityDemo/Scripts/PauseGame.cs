using UnityEngine;
using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class PauseGame : MonoBehaviour
{

    public static bool gamePaused = false;
    private static GameObject pauseGameText;
    private static UnityStandardAssets.ImageEffects.BlurOptimized blurCameraScript;

    public EventRef pauseSnapshotPath;
    public static EventInstance pauseSnapshot;
    public EventRef pauseSoundPath;
    public static EventInstance pauseSound;

    private void Start()
    {
        pauseSnapshot = RuntimeManager.CreateInstance(pauseSnapshotPath);
        pauseSound = RuntimeManager.CreateInstance(pauseSoundPath);

        pauseGameText = GameObject.Find("pauseGameText");
        blurCameraScript = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();

        unPauseGame();
    }

    private void Update()
    {
        pauseSound.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));

        if (gamePaused && Input.GetMouseButtonUp(0)) unPauseGame();
        if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) togglePause();
    }

    public static void togglePause()
    {
        if (gamePaused) unPauseGame();
        else pauseGame();
    }

    public static void unPauseGame()
    {
        gamePaused = false;
        Time.timeScale = 1;

        blurCameraScript.enabled = false;
        pauseGameText.SetActive(false);

        pauseSound.stop(STOP_MODE.IMMEDIATE);
        pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
    }

    public static void pauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;

        blurCameraScript.enabled = true;
        pauseGameText.SetActive(true);

        pauseSound.start();
        pauseSnapshot.start();
    }
}
