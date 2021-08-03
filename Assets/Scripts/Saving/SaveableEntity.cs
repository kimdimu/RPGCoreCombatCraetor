using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways] //�÷���Ÿ��, ����ƮŸ�ӿ��� ����ȴ�.
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        //[SerializeField] string uniqueIdentifier = System.Guid.NewGuid().ToString();
        //�����ĺ��ڴ� ����Ǹ� �ȵǰ� ���ӵǾ�� �Ѵ�.
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
            if (Application.IsPlaying(gameObject)) return;//�÷������̶��
            
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //�� ��δ� �������� �ǹ��Ѵ�. �׷��� �����ؾ�. �����鿡���� �ĺ��ڸ� ����ְ� �����Ѵ�.

            SerializedObject serializedobject = new SerializedObject(this);
            SerializedProperty property = serializedobject.FindProperty("uniqueIdentifier");//uniqueIdentifier ���� ����ȭ�� �Ӽ��� �޾ƿ´�.

            if (string.IsNullOrEmpty(property.stringValue))//property.stringValue == "" ��� �ΰ��� �Ѳ����� �ϴ� ǥ��
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