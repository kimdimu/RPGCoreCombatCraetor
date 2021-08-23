using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float agroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolMoveSpeed = 0.2f;
        [SerializeField] float shoutDist = 5f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevate = Mathf.Infinity;
        int patrolPathIdx = 0;

        private void Awake()
        {
            guardPosition = new LazyValue<Vector3>(GetInitGPos);
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
        }

        private Vector3 GetInitGPos()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();// = transform.position;
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimes();
        }

        public void Aggrevate()
        {
            if (timeSinceAggrevate >= 5f)
            {
                timeSinceAggrevate = 0;
                Debug.Log(timeSinceAggrevate);
                AggrevateNearbyEnemies();
            }
        }

        private void UpdateTimes()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoint += Time.deltaTime;
            timeSinceAggrevate += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                    timeSinceArriveAtWaypoint = 0;
                }
                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArriveAtWaypoint > waypointDwellTime)
                mover.StartMoveAction(nextPosition, patrolMoveSpeed);
        }

        private bool AtWaypoint()
        {
            float distToWp = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distToWp < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            patrolPathIdx = patrolPath.GetNextIndex(patrolPathIdx);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(patrolPathIdx);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDist, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            return distanceToPlayer < chaseDistance || timeSinceAggrevate < agroCooldownTime;
        }
        //call by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
