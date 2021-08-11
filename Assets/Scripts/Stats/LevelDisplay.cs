﻿using System;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", baseStats.CalculateLevel()); //소수점 어디까지?:0
        }
    }
}
