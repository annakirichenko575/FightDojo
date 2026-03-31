using UnityEngine;

namespace FightDojo.Data.AutoKeyboard
{
    public class JsonLoader
    {
        public RecordData Load(string path)
        {
            Debug.Log(path);
            string json = System.IO.File.ReadAllText(path);
            RecordData recordData = JsonUtility.FromJson<RecordData>(json);
            return recordData;
        }
    }
}
