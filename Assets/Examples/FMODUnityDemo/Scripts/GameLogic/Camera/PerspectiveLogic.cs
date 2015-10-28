using UnityEngine;

/*
* Handles switching from the player human mode and the tank game mode.
*/

namespace GameLogic
{
    public class PerspectiveLogic : MonoBehaviour
    {
        //player and tank camera rigs set in editor
        public GameObject playerCameraRig;
        public GameObject tankCameraRig;

        //player and tank objects set in editor
        public GameObject playerHuman;
        public GameObject playerTank;
        public static bool isPlayerRig = true;

        private FMODUnity.Listener humanListener;
        private FMODUnity.Listener tankListener;

        //components
        private ThirdPersonUserControl playerUserControl;
        private TankMovement tankMovement;

        private void Start()
        {
            //get components
            playerUserControl = playerHuman.GetComponent<ThirdPersonUserControl>();
            tankMovement = playerTank.GetComponent<TankMovement>();

            //get fmod listeners
            humanListener = playerHuman.GetComponent<FMODUnity.Listener>();
            tankListener = playerTank.GetComponent<FMODUnity.Listener>();

            updateRig();
        }

        private void Update()
        {
            //switch modes if V key is pressed
            //and the game isn't paused.
            if (!GamePause.isPaused && Input.GetKeyUp(KeyCode.V))
            {
                isPlayerRig = !isPlayerRig;
                updateRig();
            }

            if (!isPlayerRig)
            {
                //tankCameraRig.transform.position = playerTank.transform.position;
            }

            //lock cursor's state and visibility depending on if the game is paused or not
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
            //set active and enable/disable input depending on whether or not it
            //is the player's rig.

            playerCameraRig.SetActive(isPlayerRig);
            tankCameraRig.SetActive(!isPlayerRig);

            playerUserControl.enableInput = isPlayerRig;
            tankMovement.enableInput = !isPlayerRig;

            humanListener.enabled = isPlayerRig;
            tankListener.enabled = !isPlayerRig;
        }
    }
};