using System;
using UnityEngine;

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

            foreach (RecordedEvent e in recordData.recorded_events_v2)
            {
                //e.Log();
            }

            return recordData;
        }
    }
}
