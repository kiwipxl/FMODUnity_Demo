using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class GameDecider : MonoBehaviour
    {
        public GameObject gameOverScreenGroup;
        public Text resultsText;
        public GameObject playerTank;
        public GameObject[] enemyTanks;

        private Vector3 playerTankSpawn;
        private Vector3[] enemySpawnPoints;
        private bool hasVictory = false;
        private bool isGameOver = false;

        private void Start()
        {
            gameOverScreenGroup.SetActive(false);

            playerTankSpawn = playerTank.transform.position;

            enemySpawnPoints = new Vector3[enemyTanks.Length];
            for (int n = 0; n < enemyTanks.Length; ++n)
            {
                enemySpawnPoints[n] = enemyTanks[n].transform.position;
            }
        }

        private void Update()
        {
            if (!playerTank.activeSelf)
            {
                isGameOver = true;
                hasVictory = false;
            }

            bool allDestroyed = true;
            foreach (GameObject enemyTank in enemyTanks)
            {
                if (enemyTank.activeSelf) allDestroyed = false;
            }
            if (allDestroyed)
            {
                isGameOver = true;
                hasVictory = true;
            }

            gameOverScreenGroup.SetActive(isGameOver && !GamePause.isPaused && !PerspectiveLogic.isPlayerRig);

            if (gameOverScreenGroup.activeSelf && Input.GetKeyDown(KeyCode.R))
            {
                if (hasVictory)
                {
                    resultsText.text = "VICTORY";
                }
                else
                {
                    resultsText.text = "GAME OVER";
                }

                isGameOver = false;

                playerTank.transform.position = playerTankSpawn;
                playerTank.SetActive(true);
                for (int n = 0; n < enemyTanks.Length; ++n)
                {
                    enemyTanks[n].SetActive(true);
                    enemyTanks[n].transform.position = enemySpawnPoints[n];
                }
            }
        }
    }
};
