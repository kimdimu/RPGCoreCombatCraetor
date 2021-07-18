using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] Transform target;

        private void Update()
        {
            if (target == null) return;
            if (!GetIsInRange())
                GetComponent<Mover>().MoveTo(target.position);
            else
                GetComponent<Mover>().Stop();
            //if (target != null)
            //{
            //    bool isInRange = Vector3.Distance(target.position, transform.position) > weaponRange;
            //    if (isInRange)
            //        GetComponent<Mover>().MoveTo(target.position);
            //    else
            //        GetComponent<Mover>().Stop();
            //}
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.position, transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
