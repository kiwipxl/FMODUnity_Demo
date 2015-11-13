using UnityEngine;
using UnityEngine.UI;

/*
* Handles shell shooting logic for the tank in-game.
*/

namespace GameLogic
{
    public class TankShooting : MonoBehaviour
    {
        public int PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody Shell;                   // Prefab of the shell.
        public Transform FireTransform;           // A child of the tank where the shells are spawned.
        public Slider AimSlider;                  // A child of the tank that displays the current launch force.
        public float MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

        private float CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool Fired;                       // Whether or not the shell has been launched with this button press.
        private TankAudio tankAudio;
        private TankMovement tankMovement;

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            CurrentLaunchForce = MinLaunchForce;
            AimSlider.value = MinLaunchForce;
        }


        private void Start ()
        {
            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;

            tankAudio = GetComponent<TankAudio>();
            tankMovement = GetComponent<TankMovement>();
        }

        private void Update()
        {
            if (!enabled || !tankMovement || GamePauseLogic.isPaused) return;

            if (!tankMovement.enableInput)
            {
                if (!Fired) Fire();
                return;
            }

            // The slider should have a default value of the minimum launch force.
            AimSlider.value = MinLaunchForce;

            // If the max force has been exceeded and the shell hasn't yet been launched...
            if (CurrentLaunchForce >= MaxLaunchForce && !Fired)
            {
                // ... use the max force and launch the shell.
                CurrentLaunchForce = MaxLaunchForce;
                Fire();
            }
            // Otherwise, if the fire button has just started being pressed...
            else if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                // ... reset the fired flag and reset the launch force.
                Fired = false;
                CurrentLaunchForce = MinLaunchForce;

                //todo: richman
            }
            else if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && !Fired)
            {
                CurrentLaunchForce += ChargeSpeed * Time.deltaTime;

                AimSlider.value = CurrentLaunchForce;
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            else if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && !Fired)
            {
                // ... launch the shell.
                Fire();
            }
        }

        public void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (Shell, FireTransform.position, FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = CurrentLaunchForce * FireTransform.forward;

            shellInstance.GetComponent<ShellExplosion>().tankParent = gameObject;
            shellInstance.GetComponent<ShellAudio>().playShellFire();

            // Reset the launch force.  This is a precaution in case of missing button events.
            CurrentLaunchForce = MinLaunchForce;
        }
    }
}