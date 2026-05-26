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
    private List<ComboQualityData> lastIdCombos = new List<ComboQualityData>();
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

    public void CalculateComboHistory(int comboID)
    {
      lastIdCombos = allCombos.FindAll(x =>
        x.ComboID == comboID
        && x.Quality > 0f);
    }

    public List<Vector2> GetComboPointsAll()
    {
      bool hasMultipleDays = lastIdCombos 
        .Select(x => new DateTime(x.DateTicks).Date)
        .Distinct()
        .Count() > 1;

      List<Vector2> points = new List<Vector2>();
      if (hasMultipleDays)
      {
        points = GetComboPoints(GetHistoryByAllDays(lastIdCombos));
      }
      else
      {
        points = GetComboPoints(lastIdCombos);
      }
      return GetNormalizedComboPoints(points);
    }
    
    public List<Vector2> GetComboPointsByDay()
    {
      List<Vector2> points = GetComboPoints(
        GetHistoryByDay(lastIdCombos));
      return GetNormalizedComboPoints(points);
    }

    public List<Vector2> GetComboPointsByWeek()
    {
      List<Vector2> points = GetComboPoints(
        GetHistoryByWeek(lastIdCombos));
      return GetNormalizedComboPoints(points);
    }
  
    public List<Vector2> GetComboPointsByMonth()
    {
      List<Vector2> points = GetComboPoints(
        GetHistoryByMonth(lastIdCombos));
      return GetNormalizedComboPoints(points);
    }

    private List<Vector2> GetComboPoints(List<ComboQualityData> history)
    {
      if (history.Count == 0)
        return new List<Vector2>();

      long firstTicks = history[0].DateTicks;

      return history
        .Select(e => new Vector2(
          (e.DateTicks - firstTicks) / (float)TimeSpan.TicksPerSecond, // секунды от первой точки
          e.Quality))
        .ToList();
    }

    private List<Vector2> GetNormalizedComboPoints(List<Vector2> points)
    {
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

    private List<ComboQualityData> GetHistoryByDay(List<ComboQualityData> history)
    {
       return history 
        .Where(x => new DateTime(x.DateTicks).Date == DateTime.Today)
        .ToList(); 
    }
    
    private List<ComboQualityData> GetHistoryByWeek(List<ComboQualityData> history)
    {
       return history 
        .Where(x => new DateTime(x.DateTicks).Date >= DateTime.Today.AddDays(-7))
        .GroupBy(x => new DateTime(x.DateTicks).Date)
        .Select(g => g.OrderBy(x => x.DateTicks).Last())
        .ToList(); 
    }

    private List<ComboQualityData> GetHistoryByMonth(List<ComboQualityData> history)
    {
      return history 
        .Where(x => new DateTime(x.DateTicks).Date >= DateTime.Today.AddDays(-30))
        .GroupBy(x => new DateTime(x.DateTicks).Date)
        .Select(g => g.OrderBy(x => x.DateTicks).Last())
        .ToList();  
    }
    
    private List<ComboQualityData> GetHistoryByAllDays(List<ComboQualityData> history)
    {
      return history 
        .GroupBy(x => new DateTime(x.DateTicks).Date)
        .Select(g => g.OrderBy(x => x.DateTicks).Last())
        .ToList();  
    }
  }
}