using RPG.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text healthValueText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthValueText = GetComponent<Text>();
            healthValueText.text = String.Format("N/A %");
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                healthValueText.text = "N/A";
            }
            else
            {
                Health health = fighter.GetTarget();
                GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints()); //소수점 어디까지?:0
            }
        }
    }
}
