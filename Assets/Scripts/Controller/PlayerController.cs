using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (GetComponent<Health>().IsDead()) return;
            if (InteractWithCombat())
            {
                return;
            }
            if(InteractWithMovement())
            {
                return;
            }
            //print("nothing");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if(!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                    return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);//out is keyword
            if (hasHit)
            {
                //print(hit.transform.name);

                if (Input.GetMouseButton(0))
                {
                    //GetComponent<Fighter>().Cancel();
                    //GetComponent<Mover>().MoveTo(hit.point);
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false; 
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
