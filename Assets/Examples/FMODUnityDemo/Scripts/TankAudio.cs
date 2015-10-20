using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;

namespace Complete
{
    public class TankAudio : MonoBehaviour
    {
        public EventRef idleEnginePath;
        public EventRef enginePath;
        public EventRef treadRollingPath;
        public EventRef shellFirePath;

        private EventInstance idleEngine;
        private EventInstance engine;
        private EventInstance treadRolling;

        private TankShooting tankShooting;
        private CollidingLayers collidingLayers = new CollidingLayers();

        private void Start()
        {
            tankShooting = GetComponent<TankShooting>();

            idleEngine = RuntimeManager.CreateInstance(idleEnginePath);
            idleEngine.start();
            engine = RuntimeManager.CreateInstance(enginePath);
            engine.start();
            treadRolling = RuntimeManager.CreateInstance(treadRollingPath);
            treadRolling.start();
        }

        private void Update()
        {
            if (collidingLayers.contains("Sand"))  treadRolling.setParameterValue("T_Surface", 0);
            if (collidingLayers.contains("Water")) treadRolling.setParameterValue("T_Surface", 1);
            collidingLayers.reset();

            engine.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
            idleEngine.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
            treadRolling.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
        }

        public void updateDriving(float normalisedSpeed, float forwardInput, float turnInput)
        {
            treadRolling.setParameterValue("Speed", Mathf.Abs(turnInput) * 2000.0f);

            engine.setParameterValue("RPM", normalisedSpeed * 2000.0f);
            engine.setParameterValue("Load", forwardInput);

            idleEngine.setParameterValue("RPM", (1 - normalisedSpeed) * 800.0f);
        }

        public void fireShell(Vector3 position)
        {
            RuntimeManager.PlayOneShot(shellFirePath, Camera.main.transform.position);
        }

        private void OnTriggerStay(Collider col)
        {
            //add colliding layer
            collidingLayers.add(col.gameObject.layer);
        }

        private void OnCollisionStay(Collision col)
        {
            //add colliding layer
            collidingLayers.add(col.gameObject.layer);
        }
    }
}