using System;
using UnityEngine;

namespace FightDojo.Data.Auto_Keyboard
{
    [Serializable]
    public class RecordData
    {
        public RecordedEvent[] recorded_events_v2;
    }

    [Serializable]
    public class RecordedEvent
    {
        public KeyObject key_obj_s;
        public string key_name_display;
        public string action_canonical;
        public float delay_ms;

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

