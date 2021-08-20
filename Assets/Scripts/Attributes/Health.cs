using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenpPersentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        bool isDead = false;


        [Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        LazyValue<float> healthPoints;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private void Start()
        {
            healthPoints.ForceInit();
            //이 때 LazyValue에 넣은 GetInitialHealth을 실행하면서 초기화된다.
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            takeDamage.Invoke(damage);
            if (healthPoints.value <= 0)
            {
                Die();
                AwardExp(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExp(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExp(GetComponent<BaseStats>().GetStat(Stat.Experience));
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());
        }
        internal float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurAction();
        }

        public void RegenerateHealth()
        {
            float regenHp = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenpPersentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHp);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            Debug.Log(healthPoints.value);
            if (healthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}
