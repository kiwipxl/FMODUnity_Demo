using UnityEngine;

namespace GameLogic
{
    public class TankAIMovement : MonoBehaviour
    {
        public GameObject playerTank;
        private NavMeshAgent agent;
        private TankShooting tankShooting;
        private float shootingTimer = 0;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            tankShooting = GetComponent<TankShooting>();
        }

        private void Update()
        {
            if (!PerspectiveLogic.isPlayerRig)
            {
                agent.destination = playerTank.transform.position;

                shootingTimer += Time.deltaTime;
                if (Vector3.Distance(transform.position, playerTank.transform.position) <= 10) {
                    if (shootingTimer >= 2.0f)
                    {
                        shootingTimer = Random.value * .5f;
                        tankShooting.Fire();
                    }
                }
            }
        }
    }
}