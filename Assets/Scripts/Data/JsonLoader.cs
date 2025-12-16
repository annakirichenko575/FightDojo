using System;
using UnityEngine;

namespace FightDojo.Data
{
    public class JsonLoader
    {
        private string Path => Application.dataPath + "/test.json";

        private RecordData recordData;

        public void Load()
        {
            Debug.Log(Path);
            string json = System.IO.File.ReadAllText(Path);
            recordData = JsonUtility.FromJson<RecordData>(json);

            foreach (RecordedEvent e in recordData.recorded_events_v2)
            {
                e.Log();
            }
        }
    }
}
