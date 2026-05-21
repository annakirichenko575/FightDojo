using System.Collections.Generic;
using UnityEngine;

namespace FightDojo.Data.AutoKeyboard
{
    public static class RecordDataAdapter
    {
        public static RecordedKeys Adapt(RecordData recordData)
        {
            float time = 0;
            int i = 0;
            List<KeyData> keys = new List<KeyData>();
            foreach (RecordedEvent recordedEvent in recordData.recorded_events_v2)
            {
                if (recordedEvent.delay_ms < 0f 
                    || string.IsNullOrWhiteSpace(recordedEvent.key_name_display))
                    continue;
                    
                if (i > 0)
                {
                    time += recordedEvent.delay_ms / 1000f;
                }
                if (recordedEvent.action_canonical == KeyData.PressedActionName)
                {
                    keys.Add(new KeyData(i, recordedEvent.action_canonical, time, recordedEvent.key_name_display));
                    i++;
                }
            }

            return new RecordedKeys(keys);
        }
        
        
    }
}
