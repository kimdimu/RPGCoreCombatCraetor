using System;
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
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0, 1)] //0~1의 슬라이더를 인스펙터에 보여줌
        [SerializeField] float patrolMoveSpeed = 0.2f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtWaypoint = Mathf.Infinity;
        int patrolPathIdx=0;
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
            //if(Vector3.Distance(player.transform.position, transform.position)<chaseDisance)
            //{
            //    print(this.name + " should chase.");
            //}
            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
                //isSuspicion = true;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)// || (timeSinceArriveAtWaypoint < waypointDwellTime))//isspic
            {
                //suspicion
                SuspicionBehaviour();

                // fighter.Cancel();

                //if (timeSinceLastSawPlayer > suspicionTime)
                //{
                //    isSuspicion = false;
                //}
            }
            else
            {
                PatrolBehaviour();
                //timeSinceArriveAtWaypoint = waypointDwellTime;
                //fighter.Cancel();
                //GetComponent<Mover>().StartMoveAction(GameObject.FindWithTag("Player").transform.position);
            }

            UpdateTimes();
        }

        private void UpdateTimes()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoint += Time.deltaTime;
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

            if(timeSinceArriveAtWaypoint > waypointDwellTime)
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
            //if (patrolPathIdx + 1 >= patrolPath.transform.childCount)//탭탭!
            //    patrolPathIdx = 0;
            //else
            //    patrolPathIdx += 1;
        }

        private Vector3 GetCurrentWayPoint()
        {
            //return patrolPath.transform.GetChild(patrolPathIdx).position;
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

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
        //call by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
