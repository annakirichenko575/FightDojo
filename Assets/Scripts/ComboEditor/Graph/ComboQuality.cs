using System;
using System.IO;
using FightDojo.ComboEditor.Data;
using UnityEngine;

namespace FightDojo.ComboEditor.ComboEditor.Graph
{
  public class ComboQuality
  {
    public static readonly string Path = Application.persistentDataPath + "/combo_history.csv";
    
    private readonly ICurrentComboInfoService info;
    private readonly IRecordedKeysService recordedKeys;
    
    private int correctPress;
    private int allKeys;

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
      quality = Mathf.Clamp(quality, 0, 1);
      if (quality > 0f)
      {
        SaveCombo(info.ComboId, info.Game, info.Character, info.Author, quality);
      }
      return quality;
    }

    private void CreateHeaderIfNotExists()
    {
      if (!File.Exists(Path))
      {
        File.WriteAllText(Path, "ComboID;Date;Game;Character;Author;Quality%\n");
      }
      Debug.Log(Path);
    }

    private void SaveCombo(int comboId, string game,
      string character, string comboAuthor,
      float qualityPercent)
    {
      string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      string line = $"\"{comboId}\";\"{date}\";\"{game}\";\"{character}\";\"{comboAuthor}\";{qualityPercent:F4}";
        
      File.AppendAllText(Path, line + "\n");
        
      Debug.Log($"Сохранено: {comboAuthor} — {qualityPercent:F4}%");
    }
  }
}