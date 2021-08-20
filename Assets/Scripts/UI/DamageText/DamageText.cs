using System;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText;
        float damage;
        //private void Update()
        //{
        //    transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
        //}

        public void SetDmg(float damage)
        {
            damageText.text = string.Format("{0:0}",damage);
            //this.damage = damage;
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }

    }
}