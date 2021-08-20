using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField]  CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNMProjectionDist = 1f;
        [SerializeField] float maxNavPathLentgh = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (InteractWithUI())
                return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithUI()
        {
                //Debug.Log(EventSystem.current.IsPointerOverGameObject());
            if (EventSystem.current.IsPointerOverGameObject())//UI 터치 제외할 수 있다.
            {
                SetCursor(CursorType.UI); 
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false; //상호작용할 컴포넌트가 없다.
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits); //distances에 맞게 hits가 정렬된다. 
            return hits;
        }

        //private bool InteractWithCombat()
        //{
        //    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        //    foreach (RaycastHit hit in hits)
        //    {
        //        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        //        if (target == null) continue;

        //        if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            GetComponent<Fighter>().Attack(target.gameObject);
        //        }
        //        SetCursor(CursorType.Combat);
        //        return true;
        //    }
        //    return false;
        //}
        private bool InteractWithMovement()
        {
            //RaycastHit hit;
            //bool hasHit = Physics.Raycast(GetMouseRay(), out hit);//out is keyword
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                //print(hit.transform.name);

                if (Input.GetMouseButton(0))
                {
                    //GetComponent<Fighter>().Cancel();
                    //GetComponent<Mover>().MoveTo(hit.point);
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false; //아무것도 없다면

            NavMeshHit navMeshHit; //참조 유형이 아닌 구조 유형은 null일 수 없고 기본 값이 있다.
            bool hasCastToNM = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNMProjectionDist, NavMesh.AllAreas);
            if (!hasCastToNM) return false; //레이 쏜 주변 maxNMProjectionDist 범위에 내브메쉬가 없다면

            target = navMeshHit.position;

            NavMeshPath path =new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false; //완전한 경로가 아니라면? false
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLentgh) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalLength=0;
            if (path.corners.Length < 2) return totalLength;
            
            for (int i = 0; i < path.corners.Length-1; i++)
            {
                float dist = Vector2.Distance(path.corners[i], path.corners[i + 1]);
                totalLength += dist;
            }
            return totalLength;
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == cursorType) return mapping;
            }
            return cursorMappings[0];
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
