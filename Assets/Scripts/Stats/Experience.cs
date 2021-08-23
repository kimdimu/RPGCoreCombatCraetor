using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        //public delegate void onExpGained();
        public Action onExpGained;

        public void GainExp(float exp)
        {
            experiencePoints += exp;
            onExpGained();
        }

        public object CaptureState()
        {
            return GetPoint();
        }

        public void RestoreState(object state)
        {
            //Debug.Log(experiencePoints);
            experiencePoints = (float)state;
        }

        public float GetPoint()
        {
            return experiencePoints;
        }
    }
}