using UnityEngine;

/*
* A free look camera used for the player third person camera.
*
* Note: This script was taken from the Unity Standard Assets package and is slightly modified.
*/

namespace GameLogic
{
    public class FreeLookCam : MonoBehaviour
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising of 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Camera

        [SerializeField] private float moveSpeed = 1f;// How fast the rig will move to keep up with the target's position.
        [Range(0f, 10f)] [SerializeField] private float turnSpeed = 1.5f;// How fast the rig will rotate from user input.
        [SerializeField] private float turnSmoothing = 0.1f;// How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
        [SerializeField] private float tiltMax = 75f; // The maximum value of the x axis rotation of the pivot.
        [SerializeField] private float tiltMin = 45f; // The minimum value of the x axis rotation of the pivot.
        [SerializeField] private bool lockCursor = false; // Whether the cursor should be hidden and locked.
        [SerializeField] private bool verticalAutoReturn = false;// set wether or not the vertical axis should auto return

        private float lookAngle; // The rig's y axis rotation.
        private float tiltAngle; // The pivot's x axis rotation.
        private const float LookDistance = 100f; // How far in front of the pivot the character's look target is.
        private float smoothX = 0;
        private float smoothY = 0;
        private float smoothXvelocity = 0;
        private float smoothYvelocity = 0;

        protected Transform cam; // the transform of the camera
        protected Transform pivot; // the point at which the camera pivots around
        protected Vector3 lastTargetPosition;

        [SerializeField] protected Transform target; // The target object to follow
        [SerializeField] private bool autoTargetPlayer = true; // Whether the rig should automatically target the player.

        private void Awake()
        {
            // find the camera in the object hierarchy
            cam = GetComponentInChildren<Camera>().transform;
            pivot = cam.parent;
        }

        private void Update()
        {
            if (!GamePause.isPaused) HandleRotationMovement();

            // we update from here if updatetype is set to Late, or in auto mode,
            // if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic.
            if (autoTargetPlayer && (target == null || !target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            FollowTarget(Time.deltaTime);
        }

        public void FindAndTargetPlayer()
        {
            // auto target an object tagged player, if no target has been assigned
            var targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj)
            {
                target = targetObj.transform;
            }
        }

        private void FollowTarget(float deltaTime)
        {
            // Move the rig towards target position.
            transform.position = Vector3.Lerp(transform.position, target.position, deltaTime*moveSpeed);
        }

        private void HandleRotationMovement()
        {
            // Read the user input
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            // smooth the user input
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXvelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYvelocity, turnSmoothing);
            }
            else
            {
                smoothX = x;
                smoothY = y;
            }

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            lookAngle += smoothX*turnSpeed;

            // Rotate the rig (the root object) around Y axis only:
            transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);

            if (verticalAutoReturn)
            {
                // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
                // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
                // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
                tiltAngle = y > 0 ? Mathf.Lerp(0, -tiltMin, smoothY) : Mathf.Lerp(0, tiltMax, -smoothY);
            }
            else
            {
                // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
                tiltAngle -= smoothY*turnSpeed;
                // and make sure the new value is within the tilt range
                tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);
            }

            // Tilt input around X is applied to the pivot (the child of this object)
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);

        }
    }
}