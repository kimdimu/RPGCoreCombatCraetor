using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));//이 겜오브젝트를 비활성화하면 코루틴이 멈추므로 자식을 비활성화.
            }
        }

        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform) //자식의 트랜스폼도 가지고 있다.
            {
                child.gameObject.SetActive(shouldShow);
            }

        }
    }
}
