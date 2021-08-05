using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        public void Spawn(Transform hand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, hand);
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
    }
}