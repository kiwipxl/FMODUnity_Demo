using UnityEngine;
using UnityEngine.UI;
using ImageEffects;

namespace GameLogic
{
    public class MainLogic : MonoBehaviour
    {
        public enum TankGameState
        {
            MAIN_MENU, 
            GAME, 
            RESULTS
        }

        public GameObject tankCameraRig;

        public FMODAsset engP1WinsAsset;
        public FMODAsset engStartTanksAsset;
        public FMODAsset sweP1WinsAsset;
        public FMODAsset sweStartTanksAsset;

        public GameObject resultsScreenGroup;
        public GameObject pauseScreenGroup;
        public GameObject tanksScreenGroup;

        public static TankGameState gameState = TankGameState.MAIN_MENU;

        public Text resultsText;
        public GameObject playerTank;
        public GameObject[] enemyTanks;

        private Vector3 playerTankSpawn;
        private Vector3[] enemySpawnPoints;
        private bool hasVictory = false;
        private bool isGameOver = false;
        private bool playedSubtitle = false;

        private float zoomSpeed = 0;

        //ui
        public Text infoText;

        private void Start()
        {
            resultsScreenGroup.SetActive(false);

            playerTankSpawn = playerTank.transform.position;

            enemySpawnPoints = new Vector3[enemyTanks.Length];
            for (int n = 0; n < enemyTanks.Length; ++n)
            {
                enemySpawnPoints[n] = enemyTanks[n].transform.position;
            }

            unPauseGame();
        }

        private void Update()
        {
            //move the player tank rig to the tank's position if the tank is being controlled
            if (!PerspectiveLogic.isPlayerRig)
            {
                //calculate average position

                Vector3 avgPos = Vector3.zero;
                int count = 1;
                foreach (GameObject enemyTank in enemyTanks)
                {
                    if (enemyTank.activeSelf)
                    {
                        ++count;
                        avgPos += enemyTank.transform.position;
                    }
                }
                avgPos += playerTank.transform.position;
                avgPos /= count;
                avgPos.y = 0;
                tankCameraRig.transform.position = avgPos;

                //calculate average size

                Vector3 desiredLocalPos = transform.InverseTransformPoint(avgPos);
                float size = 0;

                foreach (GameObject enemyTank in enemyTanks)
                {
                    if (enemyTank.activeSelf)
                    {
                        Vector3 targetLocalPos = transform.InverseTransformPoint(enemyTank.transform.position);

                        Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                        size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
                        size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / Camera.main.aspect);
                    }
                }

                size += 4.0f;
                size = Mathf.Max(size, 6.5f);

                Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, size, ref zoomSpeed, .01f);
            }

            tanksScreenGroup.SetActive(false);
            resultsScreenGroup.SetActive(false);
            playerTank.GetComponent<TankMovement>().enableInput = false;

            if (!PerspectiveLogic.isPlayerRig) {
                if (!GamePause.isPaused)
                {
                    switch (gameState)
                    {
                        case TankGameState.MAIN_MENU:
                            if (!playedSubtitle)
                            {
                                playedSubtitle = true;
                                Subtitles.start(Random.value > .5 ? engStartTanksAsset : sweStartTanksAsset);
                            }

                            tanksScreenGroup.SetActive(true);

                            if (Input.GetKeyDown(KeyCode.Return))
                            {
                                gameState = TankGameState.GAME;
                                playedSubtitle = false;

                                playerTank.transform.position = playerTankSpawn;
                                playerTank.SetActive(true);
                                for (int n = 0; n < enemyTanks.Length; ++n)
                                {
                                    enemyTanks[n].SetActive(true);
                                    enemyTanks[n].transform.position = enemySpawnPoints[n];
                                    enemyTanks[n].GetComponent<Rigidbody>().velocity = Vector3.zero;
                                }
                            }

                            break;
                        case TankGameState.GAME:
                            hasVictory = false;
                            playerTank.GetComponent<TankMovement>().enableInput = true;

                            bool allDestroyed = true;
                            foreach (GameObject enemyTank in enemyTanks)
                            {
                                if (enemyTank.activeSelf) allDestroyed = false;
                            }
                            if (allDestroyed)
                            {
                                hasVictory = true;
                                gameState = TankGameState.RESULTS;
                                playedSubtitle = false;

                                if (hasVictory) {
                                    resultsText.text = "VICTORY";
                                }else {
                                    resultsText.text = "GAME OVER";
                                }
                            }

                            break;
                        case TankGameState.RESULTS:
                            if (!playedSubtitle)
                            {
                                playedSubtitle = true;
                                Subtitles.start(Random.value > .5 ? engP1WinsAsset : sweP1WinsAsset);
                            }

                            resultsScreenGroup.SetActive(true);

                            if (Input.GetKeyDown(KeyCode.Return))
                            {
                                gameState = TankGameState.MAIN_MENU;
                                playedSubtitle = false;
                            }

                            break;
                    }
                }
            }else {
                playedSubtitle = false;
                //Subtitles.stop();
            }

            if (GamePause.isPaused && Input.GetMouseButtonUp(0)) unPauseGame();
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
            pauseScreenGroup.SetActive(GamePause.isPaused);

            infoText.enabled = !GamePause.isPaused;
        }
    }
};
