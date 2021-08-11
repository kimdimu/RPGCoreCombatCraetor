using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text healthValueText;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValueText = GetComponent<Text>();
        }

        private void Update()
        {
            //healthValueText.text = String.Format("{0}%", health.GetPercentage();
            //GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage()); //소수점 어디까지?:0
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints() , health.GetMaxHealthPoints()); 
        }
    }
}
