using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

/*
* Handles switching from the player human mode and the tank game mode.
*/

namespace GameLogic
{
    public class PerspectiveLogic : MonoBehaviour
    {
        //camera rigs set in editor
        public GameObject playerCameraRig;
        public GameObject tankCameraRig;

        //player and tank set in editor
        public GameObject playerHuman;
        public GameObject playerTank;

        //ui
        public Text modeText;

        //listeners set in editor
        private StudioListener humanListener;
        private StudioListener tankListener;

        //misc
        public static bool isPlayerRig = true;

        //components
        private ThirdPersonUserControl playerUserControl;
        private TankMovement tankMovement;

        private void Start()
        {
            //get components
            playerUserControl = playerHuman.GetComponent<ThirdPersonUserControl>();
            tankMovement = playerTank.GetComponent<TankMovement>();

            //get fmod listeners
            humanListener = playerHuman.GetComponent<StudioListener>();
            tankListener = playerTank.GetComponent<StudioListener>();

            updatePerspective();
        }

        private void Update()
        {
            //switch modes if V key is pressed
            //and the game isn't paused.
            if (!GamePauseLogic.isPaused && Input.GetKeyUp(KeyCode.V))
            {
                isPlayerRig = !isPlayerRig;
                updatePerspective();
            }

            //lock cursor's state and visibility depending on if the game is paused or not
            if (GamePauseLogic.isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (!isPlayerRig)
            {
                tankCameraRig.transform.position = playerTank.transform.position;
            }
        }

        private void updatePerspective()
        {

            //set active and enable/disable input depending on whether or not it
            //is the player's rig.

            playerCameraRig.SetActive(isPlayerRig);
            tankCameraRig.SetActive(!isPlayerRig);

            playerUserControl.enableInput = isPlayerRig;
            tankMovement.enableInput = !isPlayerRig;

            //enabled/disable listeners while making sure that 
            //both listeners are not enabled at the same time
            if (isPlayerRig) {
                tankListener.enabled = !isPlayerRig;
                humanListener.enabled = isPlayerRig;
            }else { 
                humanListener.enabled = isPlayerRig;
                tankListener.enabled = !isPlayerRig;
            }

            humanListener.enabled = isPlayerRig;
            tankListener.enabled = !isPlayerRig;

            if (isPlayerRig) modeText.text = "Player";
            else modeText.text = "Tank";
        }
    }
};