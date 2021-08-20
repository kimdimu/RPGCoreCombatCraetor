using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks= 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon;

        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Health target;
        LazyValue<Weapon> currentWeapon = null;

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(GetInitialWeapon);
        }
        private void Start()
        {
            if(currentWeapon ==null)
            {
                currentWeapon.ForceInit();
            }
        }
        private Weapon GetInitialWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
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

        public void EquipWeapon(Weapon weapon)
        {
            //if (weapon == null) return;
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
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

        public bool CanAttack(GameObject combattarget)
        {
            if (combattarget == null) return false;
            Health targetToTest = combattarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();

            //if (!targetToTest.IsDead() && combattarget != null)
            //    return true;
            //return false;
        }

        private void TriggrAttack()
        {
            GetComponent<Animator>().ResetTrigger("comeback");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //ani event
        void Hit()
        {
            if (target == null) return;
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            Debug.Log(damage);
            if (currentWeapon.value.HasProjecttile())
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < currentWeapon.value.GetWeaponRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        public IEnumerable<float> GetAdditiveModifires(Stat stat) //foreach 문에 쓰기 편하다. 여기서 리턴할 값들 리스트를 만든다. 이를 tor가 받아서 리턴하게 된다.
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetWeaponDamage(); //return type = IEnumerable<float>
            }
        }

        public IEnumerable<float> GetPercentageModifires(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetWeaponPercentageBouns(); //return type = IEnumerable<float>
            }
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("comeback");
        }

        public object CaptureState()
        {
            Debug.Log(currentWeapon.value.name);
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponname = (string)state;
            Debug.Log("RestoreState: "+weaponname);
            Weapon weapon = Resources.Load<Weapon>(weaponname);
            EquipWeapon(weapon);
        }
    }
}
