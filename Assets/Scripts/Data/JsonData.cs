using System;
using System.Collections.Generic;
using UnityEngine;

namespace FightDojo.Data.Auto_Keyboard
{
    [Serializable]
    public class RecordData
    {
        public RecordedEvent[] recorded_events_v2;

        private Dictionary<int, RecordedEvent> editorStrip = new Dictionary<int, RecordedEvent>();

        public void Initialize()
        {
            for (int i = 0; i < recorded_events_v2.Length; i++)
            {
                recorded_events_v2[i].Initialize(i);
                editorStrip.Add(i, recorded_events_v2[i]);
            }
        }

        public RecordedEvent GetEditorStripItem(int i)
        {
            return editorStrip[i];
        }
    }

    [Serializable]
    public class RecordedEvent
    {
        public int id;
        public KeyObject key_obj_s;
        public string key_name_display;
        public string action_canonical;
        public float delay_ms;

        public void Initialize(int i)
        {
            id = i; 
        }

        /*public void Log()
        {
            Debug.Log($"[{action_canonical}] {key_name_display} delay={delay_ms}"
                    + $" key.type={key_obj_s.type} key.type={key_obj_s.type}");
                
        }*/
    }

    [Serializable]
    public class KeyObject
    {
        public string type;
        public string value;
    }
}

