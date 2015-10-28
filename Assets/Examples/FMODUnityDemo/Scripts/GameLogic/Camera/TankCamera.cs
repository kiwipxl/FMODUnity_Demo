using UnityEngine;

/*
* Handles moving the player tank camera to the view of the player
*/

namespace GameLogic
{
    public class TankCamera : MonoBehaviour
    {
        public GameObject playerTank;
        public float cameraSpeed = .1f;

        private void Update()
        {
            //lerp the camera's parent object to the position of the player tank
            Camera.main.transform.parent.position =
                Vector3.Lerp(Camera.main.transform.parent.position, playerTank.transform.position, cameraSpeed);
        }
    }
}