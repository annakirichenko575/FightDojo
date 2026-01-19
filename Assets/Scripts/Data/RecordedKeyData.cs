using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FightDojo.Data
{
    public class RecordedKeys
    {
        private Dictionary<int, KeyData> editorStrip = new Dictionary<int, KeyData>();

        public RecordedKeys(List<KeyData> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                editorStrip.Add(keys[i].Id, keys[i]);
            }
        }

        public KeyData GetEditorStripItem(int i)
        {
            return editorStrip[i];
        }

        public ReadOnlyCollection<KeyData> GetKeys()
        {
            return editorStrip.Values.ToList().AsReadOnly();
        }
    }

    [Serializable]
    public class KeyData
    {
        public int Id;
        public string Action;
        public float Time;
        public string KeyName;

        public KeyData(int id, string action, float time, string keyName)
        {
            Id = id;
            Action = action;
            Time = time;
            KeyName = keyName;
        }

        public void Set(string action, float time, string keyName)
        {
            Action = action;
            Time = time;
            KeyName = keyName;
        }

        public override string ToString() => $"[{Action}] key={KeyName} time={Time}";
    }
}
