using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FightDojo.ComboEditor.ComboEditor.Graph
{
  public class ComboHistoryLoader
  {
    private List<ComboQualityData> allCombos = new List<ComboQualityData>();
    private ComboHistoryValidator validator = new ComboHistoryValidator();

    public class ComboQualityData
    {
      public int ComboID;
      public long DateTicks;
      public string Game;
      public string Character;
      public string Author;
      public float Quality;

      public DateTime Date => new DateTime(DateTicks);
    }

    public void LoadCSV(string path)
    {
      if (!File.Exists(path))
        return;

      if (allCombos.Count == 0)
      {
        validator.Clean(path);
      }

      allCombos.Clear();

      string[] lines = File.ReadAllLines(path);

      for (int i = 1; i < lines.Length; i++) // пропускаем заголовок
      {
        string[] values = lines[i].Split(';');
        if (values.Length < 6)
          continue;

        try
        {
          ComboQualityData qualityData = new ComboQualityData
          {
            ComboID = int.Parse(values[0].Trim('"')),
            DateTicks = DateTime.Parse(values[1].Trim('"')).Ticks,
            Game = values[2].Trim('"'),
            Character = values[3].Trim('"'),
            Author = values[4].Trim('"'),
            Quality = float.Parse(values[5])
          };
          allCombos.Add(qualityData);
        }
        catch
        {
          continue;
        }
      }

      allCombos = allCombos.OrderBy(e => e.DateTicks).ToList();
    }

    public List<Vector2> GetNormalizedComboPoints(int comboID)
    {
      var points = GetComboPoints(comboID);

      foreach (var item in points)
        Debug.Log(item.x);

      if (points.Count == 0)
        return points;

      float minX = points.Min(p => p.x);
      float maxX = points.Max(p => p.x);

      if (Mathf.Approximately(maxX, minX))
        return points.Select(p => new Vector2(0f, p.y)).ToList();

      float diffX = maxX - minX;
      return points
        .Select(p => new Vector2((p.x - minX) / diffX, p.y))
        .ToList();
    }

    public List<ComboQualityData> GetComboHistory(int comboID)
    {
      return allCombos.FindAll(x =>
        x.ComboID == comboID
        && x.Quality > 0f);
    }

    private List<Vector2> GetComboPoints(int comboID)
    {
      List<ComboQualityData> history = GetComboHistory(comboID);
      if (history.Count == 0)
        return new List<Vector2>();

      long firstTicks = history[0].DateTicks;

      return history
        .Select(e => new Vector2(
          (e.DateTicks - firstTicks) / (float)TimeSpan.TicksPerSecond, // секунды от первой точки
          e.Quality))
        .ToList();
    }
  }
}