using UnityEngine;
using System.Collections;
using FMODUnityDemo.Characters.ThirdPerson;
using Complete;

public class PerspectiveSwitch : MonoBehaviour {

    GameObject playerCameraRig;
    GameObject tankCameraRig;
    bool isPlayerRig = true;
    ThirdPersonUserControl playerUserControl;
    TankMovement tankMovement;

    void Start() {
        playerCameraRig = GameObject.Find("playerCameraRig");
        tankCameraRig = GameObject.Find("tankCameraRig");
        playerUserControl = GameObject.Find("characterBase").GetComponent<ThirdPersonUserControl>();
        tankMovement = GameObject.Find("tank").GetComponent<TankMovement>();

        updateRig();
	}

    void Update() {
        if (!PauseGame.gamePaused && Input.GetKeyUp(KeyCode.V))
        {
            isPlayerRig = !isPlayerRig;
            updateRig();
        }

        if (!isPlayerRig)
        {
            tankCameraRig.transform.position = GameObject.Find("tank").transform.position;
        }

        if (PauseGame.gamePaused)
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

    void updateRig()
    {
        playerCameraRig.SetActive(isPlayerRig);
        playerUserControl.enableInput = isPlayerRig;

        tankCameraRig.SetActive(!isPlayerRig);
        tankMovement.enableInput = !isPlayerRig;
    }
}
