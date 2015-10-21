using UnityEngine;
using System.Collections;
using FMODUnityDemo.Characters.ThirdPerson;
using Complete;

public class PerspectiveSwitch : MonoBehaviour {

    private GameObject playerCameraRig;
    private GameObject tankCameraRig;
    public static bool isPlayerRig = true;
    private ThirdPersonUserControl playerUserControl;
    private TankMovement tankMovement;

    private void Start() {
        playerCameraRig = GameObject.Find("playerCameraRig");
        tankCameraRig = GameObject.Find("tankCameraRig");
        playerUserControl = GameObject.Find("characterBase").GetComponent<ThirdPersonUserControl>();
        tankMovement = GameObject.Find("tank").GetComponent<TankMovement>();

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
            tankCameraRig.transform.position = GameObject.Find("tank").transform.position;
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
