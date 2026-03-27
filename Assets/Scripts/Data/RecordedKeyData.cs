using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace FightDojo.Data
{
    public class RecordedKeys : IRecordedKeysService
    {
        private Dictionary<int, KeyData> _editorStrip = new Dictionary<int, KeyData>();

        private int _maxId = 0;
        
        public RecordedKeys(List<KeyData> keys)
        {
            Initialize(keys);
        }

        private void Initialize(List<KeyData> keys)
        {
            _maxId = 0;
            _editorStrip.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                if (_maxId < keys[i].Id)
                {
                    _maxId = keys[i].Id;
                }
                _editorStrip.Add(keys[i].Id, keys[i]);
            }
        }

        public KeyData GetEditorStripItem(int i) => 
            new KeyData(_editorStrip[i]);

        public void Add(KeyData keyData)
        {
            _maxId++;
            keyData.Id = _maxId;
            _editorStrip.Add(_maxId, new KeyData(keyData));
        }

        public void Delete(int id)
        {
            Debug.Log("Delete" + id + " " + _editorStrip[id].Id);
            
            _editorStrip.Remove(id);
        }

        public ReadOnlyCollection<KeyData> GetKeys()
        {
            return _editorStrip.Values
                .OrderBy(k => k.Id)
                .ToList()
                .AsReadOnly();
        }

        public void UpdateKeyName(int id, string keyName)
        {
            _editorStrip[id].KeyName = keyName;
        }

        public void UpdateKeyTime(int id, float keyTime)
        {
            _editorStrip[id].Time = keyTime;
        }

        public string ToJson()
        {
            return RecordedStripJson.ToJson(GetKeys().ToList());
        }

        public void LoadJson(string comboJson)
        {
            List<KeyData> keys = RecordedStripJson.FromJson(comboJson);
            Initialize(keys);
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

        public KeyData(KeyData keyData) : 
            this(keyData.Id, keyData.Action, 
                keyData.Time, keyData.KeyName)
        {
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
