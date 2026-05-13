using System.Collections.Generic;
using Services;
using UnityEngine;

namespace FightDojo.ComboEditor.Graph
{
  public class ComboQualityGraph : MonoBehaviour
  {
    [SerializeField] private GraphBuilder graph;
    [SerializeField] private List<Vector2> points = new List<Vector2>(); // точки X,Y от 0 до 1

    private ICurrentComboInfoService info;
    private ComboHistoryLoader historyLoader = new ComboHistoryLoader();

    private string Path => ComboQuality.Path;
    
    public void DrawGraph()
    {
      info = AllServices.Container.Single<ICurrentComboInfoService>();
      historyLoader.LoadCSV(Path);
      List<Vector2> points = 
        historyLoader.GetNormalizedComboPoints(info.ComboId);
      
      this.points = points;
      foreach (var item in historyLoader.GetComboHistory(info.ComboId))
      {
        Debug.Log(item.Date + " " + item.DateTicks);
      }
      
      
      graph.DrawGraph(points);
    }
  
  }
}
