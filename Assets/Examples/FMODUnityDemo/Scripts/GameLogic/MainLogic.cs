using UnityEngine;
using UnityEngine.UI;
using ImageEffects;

namespace GameLogic
{
    public class MainLogic : MonoBehaviour
    {
        public GameObject gameUIGroup;
        public GameObject pauseUIGroup;
        public Button engButton;
        public Button sweButton;

        private void Start()
        {
            GetComponent<GamePause>().init();

            unPauseGame();

            engButton.onClick.AddListener(() =>
            {
                engButton.interactable = false;
                sweButton.interactable = true;

                GetComponent<LocalisationVO>().switchBankTo(VOLanguage.ENGLISH);
            });
            sweButton.onClick.AddListener(() =>
            {

                engButton.interactable = true;
                sweButton.interactable = false;

                GetComponent<LocalisationVO>().switchBankTo(VOLanguage.SWEDISH);
            });
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape)) togglePause();
        }

        public void togglePause()
        {
            if (GamePause.isPaused) unPauseGame();
            else pauseGame();
        }

        public void unPauseGame()
        {
            //set time to 1
            GamePause.isPaused = false;
            Time.timeScale = 1;

            GetComponent<GamePause>().unPauseGame();

            pauseActive();
        }

        public void pauseGame()
        {
            //set time to 0
            GamePause.isPaused = true;
            Time.timeScale = 0;

            GetComponent<GamePause>().pauseGame();

            pauseActive();
        }

        private void pauseActive()
        {
            Camera.main.GetComponent<BlurOptimized>().enabled = GamePause.isPaused;
            pauseUIGroup.SetActive(GamePause.isPaused);

            gameUIGroup.SetActive(!GamePause.isPaused);
            pauseUIGroup.SetActive(GamePause.isPaused);
        }
    }
};
