using FightDojo.UI.Windows;
using FightDojo.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightDojo.ComboEditor.ComboEditor.Graph
{
  public class ResetProgressButton : MonoBehaviour
  {
    [SerializeField] private GraphWindow graphWindow;

    private ComboHistoryValidator validator = new ComboHistoryValidator();

    private ICurrentComboInfoService CurrentComboInfoService =>
      AllServices.Container.Single<ICurrentComboInfoService>();

    private string ComboQualityPath => ComboQuality.Path;

    public void ResetProgress()
    {
      validator.Clean(ComboQualityPath, CurrentComboInfoService.ComboId);
      graphWindow.OpenGraph();
    }
  }
}