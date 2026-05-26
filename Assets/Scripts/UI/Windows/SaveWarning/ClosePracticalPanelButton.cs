using FightDojo.ComboEditor;
using UnityEngine;

namespace FightDojo.UI.Windows.SaveWarning
{
  public class ClosePracticalPanelButton : MonoBehaviour
  {
    [SerializeField] private CanvasRoots canvasRoots;
    [SerializeField] private SaveWarningWindow saveWarningWindow;
    [SerializeField] private EditorComboStrip editorComboStrip;

    public void Apply()
    {
      Debug.Log(editorComboStrip.IsChanged);
      if (editorComboStrip.IsChanged)
      {
        saveWarningWindow.Open();
      }
      else
      {
        canvasRoots.OpenDbCanvas();
      }
    }
  }
}