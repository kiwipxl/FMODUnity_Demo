using UnityEngine;

namespace GameLogic
{
    public class TankMovement : MonoBehaviour
    {
        public float velocityAcceleration = 320.0f;          // How fast the tank moves forward and back.
        public float turnSpeed = 2.0f;                      // How fast the tank turns per second (in degrees).
        public float maxVelocity = 10.0f;

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

            Vector3 movement = transform.forward * verticalInput * velocityAcceleration * Time.deltaTime;
            Vector3 vel = rigidBody.velocity;
            vel += movement;
            vel.x = Mathf.Clamp(vel.x, -maxVelocity, maxVelocity);
            vel.z = Mathf.Clamp(vel.z, -maxVelocity, maxVelocity);
            rigidBody.velocity = vel;

            float maxMovement = Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.z)) / 15.0f;

            Vector3 rota = rigidBody.rotation.eulerAngles;
            rota.y += Mathf.Min(maxMovement, .2f) * turnInput * turnSpeed;
            Quaternion qrota = rigidBody.rotation;
            qrota.eulerAngles = rota;
            rigidBody.rotation = qrota;

            tankAudio.updateDriving(maxMovement, verticalInput, turnInput);
        }
    }
}