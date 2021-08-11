using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience exp;

        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            //healthValueText.text = String.Format("{0}%", health.GetPercentage();
            GetComponent<Text>().text = String.Format("{0:0}", exp.GetPoint()); //소수점 어디까지?:0
        }
    }
}
