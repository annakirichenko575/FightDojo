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

        public void Delete(int id)
        {
            editorStrip.Remove(id);
        }

        public ReadOnlyCollection<KeyData> GetKeys()
        {
            return editorStrip.Values
                .OrderBy(k => k.Id)
                .ToList()
                .AsReadOnly();
        }

        public void UpdateKeyName(int id, string keyName)
        {
            editorStrip[id].KeyName = keyName;
        }

        public void UpdateKeyTime(int id, float keyTime)
        {
            editorStrip[id].Time = keyTime;
        }
    }

    [Serializable]
    public class KeyData
    {
        public const string IsPressedAction = "press";
        public const string IsReleaseAction = "release";

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
