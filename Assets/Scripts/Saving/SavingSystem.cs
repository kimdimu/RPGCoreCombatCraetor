using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                //Transform playerT = GetPlayerTransform();
                //���� ���� ���� �ʿ䰡 ����. �������� ����ȭ �޼��尡 stream�� �ٷ� �������ֱ� ����.
                //byte[] buffer = SerializeVec(playerTransform.position);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
                //�̳��� �˾Ƽ� ������� ���ֱ� ������ ���� Write�� �� �ʿ� ����.
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                //���ۿ� �бⰡ �ʿ� ��������.
                //Transform playerT = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
                //playerT.position = pos.ToVector();

            }
        }

        private object CaptureState()
        {
            //��ųʸ�<��ü�ĺ���, ������> ����
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            return state;
        }

        private void RestoreState(object state)
        {
            //��ųʸ�<��ü�ĺ���, ������> ����
            Dictionary<string, object> stateDic = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(stateDic[saveable.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        //#region
        //public IEnumerator LoadLastScene(string saveFile)
        //{
        //    Dictionary<string, object> state = LoadFile(saveFile);
        //    int buildIndex = SceneManager.GetActiveScene().buildIndex;
        //    if (state.ContainsKey("lastSceneBuildIndex"))
        //    {
        //        buildIndex = (int)state["lastSceneBuildIndex"];
        //    }
        //    yield return SceneManager.LoadSceneAsync(buildIndex);
        //    RestoreState(state);
        //}

        //public void Save(string saveFile)
        //{
        //    Dictionary<string, object> state = LoadFile(saveFile);
        //    CaptureState(state);
        //    SaveFile(saveFile, state);
        //}

        //public void Load(string saveFile)
        //{
        //    RestoreState(LoadFile(saveFile));
        //}

        //public void Delete(string saveFile)
        //{
        //    File.Delete(GetPathFromSaveFile(saveFile));
        //}

        //private Dictionary<string, object> LoadFile(string saveFile)
        //{
        //    string path = GetPathFromSaveFile(saveFile);
        //    if (!File.Exists(path))//�ش� ��ΰ� ���ٸ� return
        //    {
        //        return new Dictionary<string, object>();
        //    }
        //    using (FileStream stream = File.Open(path, FileMode.Open))//�ش� ����� ������ ����.
        //    {
        //        byte[] buffer = new byte[stream.Length];
        //        stream.Read(buffer, 0 , buffer.Length);
        //        Encoding.UTF8.GetString(buffer);
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        return (Dictionary<string, object>)formatter.Deserialize(stream);
        //    }
        //}

        //private void SaveFile(string saveFile, object state)
        //{
        //    string path = GetPathFromSaveFile(saveFile);
        //    print("Saving to " + path);
        //    using (FileStream stream = File.Open(path, FileMode.Create))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(stream, state);
        //    }
        //}

        //private void CaptureState(Dictionary<string, object> state)
        //{
        //    foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        //    {
        //        state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
        //    }

        //    state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        //}

        //private void RestoreState(Dictionary<string, object> state)
        //{
        //    foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        //    {
        //        string id = saveable.GetUniqueIdentifier();
        //        if (state.ContainsKey(id))
        //        {
        //            saveable.RestoreState(state[id]);
        //        }
        //    }
        //}
        ////saveFile ������ ��θ� �����Ѵ�.
        //private string GetPathFromSaveFile(string saveFile)
        //{
        //    //System.IO �� Path. Combine �Լ��� ���ڿ��� �����ش�.
        //    return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        //}
        //#endregion


    }
}

//create�� �ش� ����� ���Ͽ� ������ �����, ������ �����.
//stream. �����͸� ����Ʈ ������ ������ �� �ִ�.
//��Ʈ�� (stream) : ����, ��Ʈ��ũ ��� �����͸� ����Ʈ ������ �а� ���� Ŭ����
//���Ͻ�Ʈ�� :���� ������� �ٷ�� �⺻ Ŭ����. ��Ʈ���� ��ӹް� �ִ�. �����͸� ����Ʈ ������ �а� ���� ��ɿ� ���� ������� ��������.
//stream.WriteByte(102); :102�� ���� ����Ʈ�� �ٲ� ���Ͽ� �����Ѵ�. f�� ����ȴ�. 2����Ʈ ������ ������ ������.
//byte[] bytes = Encoding.UTF8.GetBytes("ddimudimu"); : string�� �Ѳ����� ���� ����Ʈ�� ���� ����. �ٵ� �ϳ��ϳ��� 1����Ʈ������.
//stream.Write(bytes,0,bytes.Length); �̷��� ��� ����.

//���Ͽ� ���ܰ� ������� �ִ�. ���ڰ� �̸����� .. ����ǰų�.
//�׶� using(���Ͽ���){���Ͽ��⼭������ϱ�}����ؼ� �߰�ȣ �ȿ����� ������ ����� �� �ְ� �Ѵ�. �׷��� file close�� ���������ȴ�. �˾Ƽ� �ݾ���