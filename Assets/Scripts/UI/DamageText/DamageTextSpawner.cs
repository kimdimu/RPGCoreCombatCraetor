using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextFab = null;
        public void Spawn(float damage)
        {
            DamageText inst = Instantiate(damageTextFab, transform);
            inst.SetDmg(damage);
        }
    }
}