using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace FightDojo.ComboEditor.Data.AutoKeyboard
{
  public class JsonLoader
  {
    public bool TryLoad(string path, out RecordData recordData)
    {
      Debug.Log(path);
      try
      {
        string json = System.IO.File.ReadAllText(path);
        recordData = JsonUtility.FromJson<RecordData>(json);
        return true;
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to load RecordData from '{path}': {e.Message}");
        recordData = new RecordData();
        return false;
      }
    }

    public bool TrySaveToJsonFile(string path, RecordData recordData)
    {
      try
      {
        string json = JsonUtility.ToJson(recordData, prettyPrint: true);
        File.WriteAllText(path, json, 
          new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        return true;
      }
      catch (Exception e)
      {
        Debug.LogWarning($"Failed to save RecordData to '{path}': {e.Message}");
        return false;
      }
    }
  }
}