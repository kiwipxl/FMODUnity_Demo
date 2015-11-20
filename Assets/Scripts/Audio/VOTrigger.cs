using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/*
* Triggers a VO if the player human goes through the sphere collider.
* The VOTriggerControl logic script handles when the VO can play or not.
*/

namespace GameLogic
{
    public class VOTrigger : MonoBehaviour
    {
        // VO Path (set in editor)
        [EventRef] public string VOPath;

        public GameObject playerHuman;

        private void OnTriggerEnter(Collider other)
        {
            // If sphere collider is triggering with player human and
            // VO can play, then start VO subtitles
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (VOTriggerControl.canPlayVO)
                {
                    Subtitles.start(VOLocalisation.createInstance(VOPath));
                    VOTriggerControl.canPlayVO = false;
                }
            }
        }
    }
};