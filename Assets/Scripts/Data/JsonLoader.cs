using System;
using UnityEngine;
using FightDojo.Data.Auto_Keyboard;
using Unity.VisualScripting;

namespace FightDojo.Data
{
    public class JsonLoader
    {
        private string Path => Application.dataPath + "/test.json";


        public RecordData Load()
        {
            Debug.Log(Path);
            string json = System.IO.File.ReadAllText(Path);
            RecordData recordData = JsonUtility.FromJson<RecordData>(json);
            recordData.Initialize();
            return recordData;
        }
    }
}
