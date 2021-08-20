using GameDevTV.Utils;
using System;
using UnityEngine;

namespace RPG.Stats
{
    //And the namespace that we can use in there will also be calling attributes instead. 여기서 사용할 수 있는 네임스페이스도 대신 속성을 호출합니다.
    //So do go ahead and use that from the get go and save yourself a bit of trouble. 그러니 처음부터 그걸 사용해서 수고를 덜도록 하세요.
    public class BaseStats : MonoBehaviour
    {
        public event Action onLevelUp;

        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifier = false;

        LazyValue<int> curLevel;
        Experience experience;
        private void Awake()
        {
            curLevel = new LazyValue<int>(CalculateLevel);
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            curLevel.ForceInit();
        }
        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExpGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExpGained -= UpdateLevel;
            }
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > curLevel.value)
            {
                curLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpParticleEffect != null)
                Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat))* (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (curLevel.value < 1)
            {
                curLevel.value = CalculateLevel();
            }
            return curLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach(IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                //GetEnumerator()가 있다면. IEnumerator를 반환받고, 이를 순회한다. yield return이 끝날 때 까지 GetAdditiveModifire를 돈다. 리턴된 리스트를 가지고 있다.
                foreach (float modifier in modifierProvider.GetAdditiveModifires(stat))//IEnumerable<float>.Get..tor() 
                {
                    //IEnumerator it = modifierProvider.GetAdditiveModifire(stat).GetEnumerator(); //IEnumerator it = currentWeapon.GetWeaponDamage();
                    //it.MoveNext(); //return damage;
                    //total += it.Current; //it.Current = float damage;
                    total += modifier;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetPercentageModifires(stat))
                { 
                    total += modifier;
                }
            }
            return total;
        }
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float curEXP = experience.GetPoint();
            int penultimateLevel = progression.GetLevelsLength(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > curEXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }


}
