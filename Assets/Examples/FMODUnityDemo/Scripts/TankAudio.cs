using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;

namespace Complete
{
    public class TankAudio : MonoBehaviour
    {
        public EventRef enginePath;
        public EventRef treadRollingPath;
        public EventRef shellFirePath;

        private EventInstance engine;
        private EventInstance treadRolling;

        private TankShooting tankShooting;

        private void Start()
        {
            tankShooting = GetComponent<TankShooting>();

            engine = RuntimeManager.CreateInstance(enginePath);
            engine.start();
            treadRolling = RuntimeManager.CreateInstance(treadRollingPath);
            treadRolling.start();
        }

        public void updateDriving(Vector3 velocity, float forwardInput, float turnInput)
        {

        }

        public void fireShell(Vector3 position)
        {
            RuntimeManager.PlayOneShot(shellFirePath, position);
        }
    }
}