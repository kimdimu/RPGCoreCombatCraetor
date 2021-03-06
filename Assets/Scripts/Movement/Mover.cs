using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent NavMeshAgent;
        [SerializeField] float maxSpeed = 5f;
        [SerializeField] float maxNavPathLentgh = 40f;

        private void Awake() 
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLentgh) return false;

            return true;
        }
        
        void Update()
        {
            NavMeshAgent.enabled = !GetComponent<Health>().IsDead();
            UpdateAnimator();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            //GetComponent<Fighter>().Cancel();
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            NavMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            NavMeshAgent.destination = destination;
            NavMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            NavMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        private float GetPathLength(NavMeshPath path)
        {
            float totalLength = 0;
            if (path.corners.Length < 2) return totalLength;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                float dist = Vector2.Distance(path.corners[i], path.corners[i + 1]);
                totalLength += dist;
            }
            return totalLength;
        }
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);//원래 벡터3 안되는데 이건 됨
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            NavMeshAgent.enabled = false;
            transform.position = position.ToVector();
            NavMeshAgent.enabled = true;
            //Debug.Log(position);
            GetComponent<ActionScheduler>().CancelCurAction();
        }
    }
}