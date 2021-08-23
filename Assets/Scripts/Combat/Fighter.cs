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
        [SerializeField] WeaponConfig defaultWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Health target;

        [SerializeField] WeaponConfig currentWeaponConfig = null;
        LazyValue<Weapon> curWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            curWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }
        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }
        private void Start()
        {
            curWeapon.ForceInit();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            else
            {
                AttackBehaviour();
                GetComponent<Mover>().Cancel();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            curWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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
            if (!GetComponent<Mover>().CanMoveTo(combattarget.transform.position) &&
                !GetIsInRange(combattarget.transform)) return false;
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

            if(curWeapon.value!=null)
            {
                curWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjecttile())
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            else if(!target.IsDead())
            {
                target.TakeDamage(gameObject, damage);
            }
        }
        void Shoot()
        {
            Hit();
        }
        private bool GetIsInRange(Transform targetTf)
        {
            return Vector3.Distance(targetTf.position, transform.position) < currentWeaponConfig.GetWeaponRange();
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
                yield return currentWeaponConfig.GetWeaponDamage(); //return type = IEnumerable<float>
            }
        }

        public IEnumerable<float> GetPercentageModifires(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetWeaponPercentageBouns(); //return type = IEnumerable<float>
            }
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("comeback");
        }

        public object CaptureState()
        {
            Debug.Log(currentWeaponConfig.name);
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponname = (string)state;
            Debug.Log("RestoreState: "+weaponname);
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponname);
            EquipWeapon(weapon);
        }
    }
}
