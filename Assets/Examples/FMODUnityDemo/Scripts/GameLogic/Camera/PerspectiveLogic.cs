using UnityEngine;

namespace GameLogic
{
    public class PerspectiveLogic : MonoBehaviour
    {
        public GameObject playerCameraRig;
        public GameObject tankCameraRig;

        public GameObject playerHuman;
        public GameObject playerTank;
        public static bool isPlayerRig = true;

        private ThirdPersonUserControl playerUserControl;
        private TankMovement tankMovement;

        private void Start()
        {
            playerUserControl = playerHuman.GetComponent<ThirdPersonUserControl>();
            tankMovement = playerTank.GetComponent<TankMovement>();

            updateRig();
        }

        private void Update()
        {
            if (!GamePause.isPaused && Input.GetKeyUp(KeyCode.V))
            {
                isPlayerRig = !isPlayerRig;
                updateRig();
            }

            if (!isPlayerRig)
            {
                tankCameraRig.transform.position = playerTank.transform.position;
            }

            if (GamePause.isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void updateRig()
        {
            playerCameraRig.SetActive(isPlayerRig);
            playerUserControl.enableInput = isPlayerRig;

            tankCameraRig.SetActive(!isPlayerRig);
            tankMovement.enableInput = !isPlayerRig;
        }
    }
};