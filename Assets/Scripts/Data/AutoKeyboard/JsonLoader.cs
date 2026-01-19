using UnityEngine;

namespace FightDojo.Data.AutoKeyboard
{
    public class JsonLoader
    {
        private string Path => Application.dataPath + "/test.json";


        public RecordData Load()
        {
            Debug.Log(Path);
            string json = System.IO.File.ReadAllText(Path);
            RecordData recordData = JsonUtility.FromJson<RecordData>(json);
            return recordData;
        }
    }
}
