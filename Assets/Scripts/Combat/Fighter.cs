using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float timeBetweenAttacks= 1f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Weapon curWeapon = null;


        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Health target;


        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }
        public void EquipWeapon(Weapon weapon)
        {
            //if (weapon == null) return;
            curWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            else
            {
                AttackBehaviour();
                GetComponent<Mover>().Cancel();
            }
        }

        public bool CanAttack(GameObject combattarget)
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
            target.TakeDamage(curWeapon.GetWeaponDamage());
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < curWeapon.GetWeaponRange();
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("comeback");
        }


    }
}
