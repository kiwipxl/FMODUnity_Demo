using UnityEngine;

/*
* Simply has a timer that after X seconds, allows VO trigger's to play
*/

namespace GameLogic
{
    public class VOTriggerControl : MonoBehaviour
    {
        // How long it will take to allow playing of VO triggers
        public int canPlayVOInSeconds = 10;

        private static float timer = 0;

        // Can play VO triggers
        public static bool canPlayVO = true;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= canPlayVOInSeconds)
            {
                canPlayVO = true;
                timer = 0;
            }
        }
    }
}