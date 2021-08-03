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
                //버퍼 또한 만들 필요가 없다. 포매터의 직렬화 메서드가 stream에 바로 저장해주기 때문.
                //byte[] buffer = SerializeVec(playerTransform.position);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
                //이놈이 알아서 저장까지 해주기 때문에 따로 Write를 할 필요 없다.
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                //버퍼와 읽기가 필요 없어진다.
                //Transform playerT = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
                //playerT.position = pos.ToVector();

            }
        }

        private object CaptureState()
        {
            //딕셔너리<객체식별자, 데이터> 상태
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            return state;
        }

        private void RestoreState(object state)
        {
            //딕셔너리<객체식별자, 데이터> 상태
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
        //    if (!File.Exists(path))//해당 경로가 없다면 return
        //    {
        //        return new Dictionary<string, object>();
        //    }
        //    using (FileStream stream = File.Open(path, FileMode.Open))//해당 경로의 파일을 연다.
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
        ////saveFile 파일의 경로를 리턴한다.
        //private string GetPathFromSaveFile(string saveFile)
        //{
        //    //System.IO 의 Path. Combine 함수로 문자열을 더해준다.
        //    return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        //}
        //#endregion


    }
}

//create은 해당 경로의 파일에 없으면 만들고, 있으면 덮어쓴다.
//stream. 데이터를 바이트 단위로 관리할 수 있다.
//스트림 (stream) : 파일, 네트워크 등에서 데이터를 바이트 단위로 읽고 쓰는 클래스
//파일스트림 :파일 입출력을 다루는 기본 클래스. 스트림을 상속받고 있다. 데이터를 바이트 단위로 읽고 쓰는 기능에 파일 입출력이 더해진것.
//stream.WriteByte(102); :102를 단일 바이트로 바꿔 파일에 저장한다. f가 저장된다. 2바이트 단위를 넣으면 깨진다.
//byte[] bytes = Encoding.UTF8.GetBytes("ddimudimu"); : string을 한꺼번에 여러 바이트로 저장 가능. 근데 하나하나가 1바이트여야함.
//stream.Write(bytes,0,bytes.Length); 이렇게 출력 가능.

//파일에 예외가 생길수도 있다. 문자가 이리저리 .. 유출되거나.
//그때 using(파일열기){파일여기서만사용하기}사용해서 중괄호 안에서만 파일을 사용할 수 있게 한다. 그러면 file close를 안해줘옫된다. 알아서 닫아줌