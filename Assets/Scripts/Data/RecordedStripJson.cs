using System.Collections.Generic;
using UnityEngine;

namespace FightDojo.Data
{
  [System.Serializable]
  public class RecordedStripJson
  {
    public List<KeyData> Strip = new List<KeyData>();
    
    
    public static string ToJson(List<KeyData> keys)
    {
        RecordedStripJson wrapper = new RecordedStripJson
        {
            Strip = new List<KeyData>(keys)   // копируем только Values
        };

        return JsonUtility.ToJson(wrapper, true);   // true = красивый формат с отступами
    }
    
    public static List<KeyData> FromJson(string json)
    {
      if (string.IsNullOrWhiteSpace(json))
        return new List<KeyData>();

      try
      {
        RecordedStripJson wrapper = JsonUtility.FromJson<RecordedStripJson>(json);
        return wrapper?.Strip ?? new List<KeyData>();
      }
      catch
      {
        Debug.LogError("Не удалось загрузить JSON");
        return new List<KeyData>();
      }
    }
  }
}