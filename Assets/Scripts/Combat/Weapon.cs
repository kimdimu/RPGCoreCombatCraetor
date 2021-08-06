using System;
using RPG.Core;
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
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                Transform handTf = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTf);
                weapon.name = weaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon ==null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING"; //테스트를 위함
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTf;
            if (isRightHanded) handTf = rightHand;
            else handTf = leftHand;
            return handTf;
        }

        public bool HasProjecttile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInst = Instantiate(projectile, GetTransform(rightHand,leftHand).position, Quaternion.identity);
            projectileInst.SetTarget(target, weaponDamage);
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