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
      List<Vector2> points =
        historyLoader.GetNormalizedComboPoints(info.ComboId);

      foreach (var item in historyLoader.GetComboHistory(info.ComboId))
      {
        Debug.Log(item.Date + " " + item.DateTicks);
      }


      graph.DrawGraph(points);
    }
  }
}