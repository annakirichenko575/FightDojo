using System;
using UnityEngine;

namespace FightDojo.Data.AutoKeyboard
{
    public class JsonLoader
    {
        public bool TryLoad(string path, out RecordData recordData)
        {
            Debug.Log(path);
            try
            {
                string json = System.IO.File.ReadAllText(path);
                recordData = JsonUtility.FromJson<RecordData>(json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load RecordData from '{path}': {e.Message}");
                recordData = new RecordData();
                return false;
            }
        }
    }
}
