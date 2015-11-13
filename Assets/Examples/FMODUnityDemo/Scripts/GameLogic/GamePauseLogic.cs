using UnityEngine;
using ImageEffects;

/*
* Handles game pause
*/

namespace GameLogic
{
    public class GamePauseLogic : MonoBehaviour
    {
        // Audio object that contains all components
        public GameObject audioMain;

        public GameObject gameUIGroup;
        public GameObject pauseUIGroup;

        public static bool isPaused = false;

        private void Start()
        {
            audioMain.GetComponent<GamePauseAudio>().init();

            unPauseGame();
        }

        private void Update()
        {
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

            audioMain.GetComponent<GamePauseAudio>().unPauseGame();

            pauseActive();
        }

        public void pauseGame()
        {
            //set time to 0
            isPaused = true;
            Time.timeScale = 0;

            audioMain.GetComponent<GamePauseAudio>().pauseGame();

            pauseActive();
        }

        private void pauseActive()
        {
            Camera.main.GetComponent<BlurOptimized>().enabled = isPaused;
            pauseUIGroup.SetActive(isPaused);

            gameUIGroup.SetActive(!isPaused);
            pauseUIGroup.SetActive(isPaused);
        }
    }
};
