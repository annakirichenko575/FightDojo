using System.Collections.Generic;
using UnityEngine;

namespace FightDojo.ComboEditor.Data.AutoKeyboard
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
    
    public static RecordData Adapt(List<KeyData> keys)
    {
      RecordedEvent[] events = new RecordedEvent[keys.Count];

      for (int i = 0; i < keys.Count; i++)
      {
        KeyData keyData = keys[i];
        float prevTime = i > 0 ? keys[i - 1].Time : 0f;
        float delay = keys[i].Time - prevTime;
        events[i] = new RecordedEvent
        {
          key_obj_s = new KeyObject
          {
            type = "keycode_char",
            value = keyData.KeyName.ToLower()
          },
          key_name_display = keyData.KeyName.ToUpper(),
          action_canonical = keyData.Action,
          delay_ms = delay * 1000f
        };
      }

      return new RecordData { recorded_events_v2 = events };
    }
  }
}