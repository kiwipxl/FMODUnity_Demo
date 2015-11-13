using UnityEngine;

namespace GameLogic
{
    public class TankMovement : MonoBehaviour
    {
        public float moveSpeed = 12.0f;         // How fast the tank moves forward and back.
        public float turnSpeed = 180.0f;        // How fast the tank turns per second (in degrees).

        public bool enableInput = true;

        private Rigidbody rigidBody;
        private TankAudio tankAudio;

        private void Awake ()
        {
            rigidBody = GetComponent<Rigidbody>();
            tankAudio = GetComponent<TankAudio>();
        }

        private void Update()
        {
            float verticalInput = 0;
            float turnInput = 0;

            if (enableInput)
            {
                verticalInput = Input.GetAxis("Vertical");
                turnInput = Input.GetAxis("Horizontal");
            }

            Vector3 movement = transform.forward * turnInput * moveSpeed * Time.deltaTime;
            Vector3 vel = rigidBody.velocity;
            //vel += movement * 100.0f;
            vel.x += 100;
            rigidBody.velocity = vel;

            Vector3 rota = transform.localEulerAngles;
            rota.y += turnInput * verticalInput;
            transform.localEulerAngles = rota;

            float maxMovement = Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.z)) / .3f;
            tankAudio.updateDriving(maxMovement, verticalInput, turnInput);
        }
    }
}