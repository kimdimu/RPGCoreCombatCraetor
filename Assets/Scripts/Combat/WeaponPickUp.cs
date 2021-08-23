using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag =="Player")
            {
                PickUp(other.gameObject);
            }
        }

        private void PickUp(GameObject other)
        {
            if (weapon != null)
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if(healthToRestore>0)
            {
                other.GetComponent<Health>().Heal(healthToRestore);
            }
                StartCoroutine(HideForSeconds(respawnTime));//이 겜오브젝트를 비활성화하면 코루틴이 멈추므로 자식을 비활성화.
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PickUp(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }
}
