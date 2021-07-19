using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks= 1f;
        float timeSinceLastAttack = 0;

        [SerializeField] Health target;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
                GetComponent<Mover>().MoveTo(target.transform.position);
            else
            {
                AttackBehaviour();
                GetComponent<Mover>().Cancel();
            }
        }

        public bool CanAttack(CombatTarget combattarget)
        {
            if (combattarget == null) return false;
            Health targetToTest = combattarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();

            //if (!targetToTest.IsDead() && combattarget != null)
            //    return true;
            //return false;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                TriggrAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggrAttack()
        {
            GetComponent<Animator>().ResetTrigger("comeback");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //ani event
        void Hit()
        {
            //target.GetComponent<Health>().TakeDamage(weaponDamage);
            //Health healthComponent = target.GetComponent<Health>();
            // healthComponent.TakeDamage(weaponDamage);
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            print("cb");
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("comeback");
        }
    }
}
