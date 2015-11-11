using UnityEngine;

namespace GameLogic
{
    public class TankAIMovement : MonoBehaviour
    {
        public GameObject playerTank;
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            agent.destination = playerTank.transform.position;
        }
    }
}