using System.Collections.Generic;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.ComboEditor.ComboEditor.Graph
{
  public class ComboQualityGraph : MonoBehaviour
  {
    [SerializeField] private GraphBuilder graph;

    private ICurrentComboInfoService info;
    private ComboHistoryLoader historyLoader = new ComboHistoryLoader();

    private string Path => ComboQuality.Path;

    public void DrawGraph()
    {
      info = AllServices.Container.Single<ICurrentComboInfoService>();
      historyLoader.LoadCSV(Path);
      historyLoader.CalculateComboHistory(info.ComboId);
      List<Vector2> points = historyLoader.GetComboPointsAll();
      graph.DrawGraph(points);
    }

    public void RedrawGraphAll()
    {
      List<Vector2> points = historyLoader.GetComboPointsAll();
      graph.DrawGraph(points);
    }
    
    public void RedrawGraphByDay()
    {
      List<Vector2> points = historyLoader.GetComboPointsByDay();
      graph.DrawGraph(points);
    }

    public void RedrawGraphByWeek()
    {
      List<Vector2> points = historyLoader.GetComboPointsByWeek();
      graph.DrawGraph(points);
    }

    public void RedrawGraphByMonth()
    {
      List<Vector2> points = historyLoader.GetComboPointsByMonth();
      graph.DrawGraph(points);
    }

  }
}