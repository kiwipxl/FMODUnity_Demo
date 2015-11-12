﻿using UnityEngine;
using UnityEngine.UI;
using ImageEffects;

namespace GameLogic
{
    public class MainLogic : MonoBehaviour
    {
        public GameObject gameUIGroup;
        public GameObject pauseUIGroup;
        public Dropdown languageDropdown;

        private void Start()
        {
            unPauseGame();
        }

        private void Update()
        {
            if (languageDropdown.captionText.text == "Swedish")
            {
                GetComponent<LocalisationVO>().switchBankTo(VOLanguage.SWEDISH);
            }
            else if (languageDropdown.captionText.text == "English")
            {
                GetComponent<LocalisationVO>().switchBankTo(VOLanguage.ENGLISH);
            }

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
