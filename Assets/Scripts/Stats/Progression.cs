using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/new Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();
            float[] levels= lookupTable[characterClass][stat];
            if(levels.Length<level)
            {
                return 0;
            }
            return levels[level-1];
        }

        public int GetLevelsLength(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass pCC in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat stat1 in pCC.stats)
                {
                    statLookupTable[stat1.stat] = stat1.levels;
                }

                lookupTable[pCC.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;

        }
        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }


}
