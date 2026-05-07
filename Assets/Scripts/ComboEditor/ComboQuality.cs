using System;
using System.IO;
using FightDojo.Data;
using UnityEngine;

namespace FightDojo
{
  public class ComboQuality
  {
    private readonly ICurrentComboInfoService info;
    private readonly IRecordedKeysService recordedKeys;
    
    private int correctPress;
    private int allKeys;
    private string path = Application.persistentDataPath + "/combo_history.csv";

    public ComboQuality(ICurrentComboInfoService currentComboInfo, IRecordedKeysService recordedKeys)
    {
      this.info = currentComboInfo;
      this.recordedKeys = recordedKeys;
      CreateHeaderIfNotExists();
    }

    public void Correct()
    {
      correctPress++;
    }

    public void Reset()
    {
      correctPress = 0;
      allKeys = recordedKeys.Count;
    }

    public float CalculateQuality()
    {
      float quality = (float)correctPress / allKeys;
      SaveCombo(info.ComboId, info.Game, info.Character, info.Author, quality);
      return quality;
    }

    private void CreateHeaderIfNotExists()
    {
      if (!File.Exists(path))
      {
        File.WriteAllText(path, "ComboID;Date;Game;Character;Author;Quality%\n");
      }
      Debug.Log(path);
    }

    private void SaveCombo(int comboId, string game,
      string character, string comboAuthor,
      float qualityPercent)
    {
      string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      string line = $"\"{comboId}\";\"{date}\";\"{game}\";\"{character}\";\"{comboAuthor}\";{qualityPercent:F4}";
        
      File.AppendAllText(path, line + "\n");
        
      Debug.Log($"Сохранено: {comboAuthor} — {qualityPercent:F4}%");
    }
  }
}