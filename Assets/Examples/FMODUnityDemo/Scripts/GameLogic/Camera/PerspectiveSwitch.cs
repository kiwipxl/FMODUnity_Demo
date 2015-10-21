using UnityEngine;
using System.Collections;

public class PerspectiveSwitch : MonoBehaviour {

    private GameObject playerCameraRig;
    private GameObject tankCameraRig;
    public static bool isPlayerRig = true;
    private GameLogic.ThirdPersonUserControl playerUserControl;
    private GameLogic.TankMovement tankMovement;
    private GameObject tank;

    private void Start() {
        playerCameraRig = GameObject.Find("playerCameraRig");
        tankCameraRig = GameObject.Find("tankCameraRig");
        playerUserControl = GameObject.Find("characterBase").GetComponent<GameLogic.ThirdPersonUserControl>();
        tank = GameObject.Find("tank");
        tankMovement = tank.GetComponent<GameLogic.TankMovement>();

        updateRig();
	}

    private void Update() {
        if (!GamePause.isPaused && Input.GetKeyUp(KeyCode.V))
        {
            isPlayerRig = !isPlayerRig;
            updateRig();
        }

        if (!isPlayerRig)
        {
            tankCameraRig.transform.position = tank.transform.position;
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
