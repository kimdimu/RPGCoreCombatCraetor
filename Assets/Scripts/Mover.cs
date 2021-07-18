using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        //[SerializeField] Transform target;
        NavMeshAgent NavMeshAgent;
        private void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            NavMeshAgent.destination = destination;
            NavMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            NavMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            print(localVelocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
