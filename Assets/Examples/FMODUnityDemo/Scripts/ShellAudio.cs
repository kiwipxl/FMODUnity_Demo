using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMOD.Studio;
using FMODUnity;

namespace Complete
{
    public class ShellAudio : MonoBehaviour
    {
        public EventRef shellExplosionPath;

        private TankShooting tankShooting;

        private void Start()
        {
            tankShooting = GetComponent<TankShooting>();
        }

        public void shellExplode(Vector3 position)
        {
            RuntimeManager.PlayOneShot(shellExplosionPath, Camera.main.transform.position);
        }
    }
}