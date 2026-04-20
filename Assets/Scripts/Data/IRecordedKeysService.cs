using System.Collections.Generic;
using System.Collections.ObjectModel;
using Services;

namespace FightDojo.Data
{
  public interface IRecordedKeysService : IService
  {
    KeyData GetEditorStripItem(int i);
    void Add(KeyData keyData);
    void Delete(int id);
    ReadOnlyCollection<KeyData> GetKeys();
    void UpdateKeyName(int id, string keyName);
    void UpdateKeyTime(int id, float keyTime);
    string ToJson();
    void LoadJson(string comboJson);
    bool FindApproximately(string keyName, float targetTime, float tolerance);
    float GetMaxTime();
    List<float> GetKeyTimes();
  }
}