using System.Collections.Generic;

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
                keys.Add(new KeyData(i, recordedEvent.action_canonical, time, recordedEvent.key_name_display));
                i++;
                time += recordedEvent.delay_ms / 1000f;
            }
            return new RecordedKeys(keys);
        }
    }
}
