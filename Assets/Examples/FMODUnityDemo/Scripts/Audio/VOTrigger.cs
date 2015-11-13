using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/*
* Triggers a VO if the player human goes through the sphere collider.
* VOTriggerControl handles when the VO can play or not.
*/

namespace GameLogic
{
    public class VOTrigger : MonoBehaviour
    {
        // VO Path (set in editor)
        [EventRef] public string VOPath;

        public EventInstance VOEvent;

        public GameObject playerHuman;

        private void Start()
        {
            VOEvent = RuntimeManager.CreateInstance(VOPath);
        }

        private void OnTriggerEnter(Collider other)
        {
            // If sphere collider is triggering with player human and
            // VO can play, then start VO subtitles
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (VOTriggerControl.canPlayVO)
                {
                    Subtitles.start(VOEvent);
                    VOTriggerControl.canPlayVO = false;
                }
            }
        }
    }
};