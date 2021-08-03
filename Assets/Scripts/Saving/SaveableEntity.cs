using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways] //플레이타임, 에디트타임에도 실행된다.
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        //[SerializeField] string uniqueIdentifier = System.Guid.NewGuid().ToString();
        //고유식별자는 변경되면 안되고 지속되어야 한다.
        //static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            //GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)state).ToVector();
            //GetComponent<NavMeshAgent>().enabled = true;
            //GetComponent<ActionScheduler>().CancelCurrentAction();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;//플레이중이라면
            
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //빈 경로는 프리펩을 의미한다. 그래서 리턴해야. 프리펩에서는 식별자를 비어있게 설정한다.

            SerializedObject serializedobject = new SerializedObject(this);
            SerializedProperty property = serializedobject.FindProperty("uniqueIdentifier");//uniqueIdentifier 에서 직렬화된 속성을 받아온다.

            if (string.IsNullOrEmpty(property.stringValue))//property.stringValue == "" 대신 두개를 한꺼번에 하는 표현
            {
                uniqueIdentifier = System.Guid.NewGuid().ToString();
                serializedobject.ApplyModifiedProperties();
            }
        }
#endif
        //#if UNITY_EDITOR
        //        private void Update() {
        //            if (Application.IsPlaying(gameObject)) return;
        //            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

        //            SerializedObject serializedObject = new SerializedObject(this);
        //            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

        //            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        //            {
        //                property.stringValue = System.Guid.NewGuid().ToString();
        //                serializedObject.ApplyModifiedProperties();
        //            }

        //            globalLookup[property.stringValue] = this;
        //        }
        //#endif

        //        private bool IsUnique(string candidate)
        //        {
        //            if (!globalLookup.ContainsKey(candidate)) return true;

        //            if (globalLookup[candidate] == this) return true;

        //            if (globalLookup[candidate] == null)
        //            {
        //                globalLookup.Remove(candidate);
        //                return true;
        //            }

        //            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
        //            {
        //                globalLookup.Remove(candidate);
        //                return true;
        //            }

        //            return false;
        //        }
    }
}