using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenpPersentage = 70f;
        [SerializeField] float healthPoints = -1f;
        bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints <= 0)
            {
                Die();
                AwardExp(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
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
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            healthPoints = Mathf.Max(healthPoints, regenHp);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
