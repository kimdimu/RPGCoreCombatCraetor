using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if(InteractWithCombat())
            {
                return;
            }
            if(InteractWithMovement())
            {
                return;
            }
            print("nothing");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(!GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
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
                print(hit.transform.name);

                if (Input.GetMouseButton(0))
                {
                    //GetComponent<Fighter>().Cancel();
                    //GetComponent<Mover>().MoveTo(hit.point);
                    GetComponent<Mover>().StartMoveAction(hit.point);
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
