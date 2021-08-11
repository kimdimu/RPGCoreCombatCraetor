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

        int curLevel = 0;

        private void Start()
        {
            curLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExpGained += UpdateLevel;
            }
            Debug.Log("BaseStats Start");
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > curLevel)
            {
                curLevel = newLevel;
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
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (curLevel < 1)
            {
                curLevel = CalculateLevel();
            }
            return curLevel;
        }

        public int CalculateLevel()
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
