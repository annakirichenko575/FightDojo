using System;
using FightDojo.ComboEditor;
using FightDojo.ComboEditor.ComboEditor.Graph;
using UnityEngine;

namespace FightDojo.UI.Windows
{
  public class GraphWindow : MonoBehaviour
  {
    [SerializeField] private GameObject _wrap;
    [SerializeField] private GameObject _graphPanel;
    [SerializeField] private ComboQualityGraph _comboQualityGraph;

    private InputComboBuilder inputComboBuilder;

    private void Awake()
    {
      inputComboBuilder = FindAnyObjectByType<InputComboBuilder>();
    }

    private void Start()
    {
      Hide();
    }

    public void OpenGraph()
    {
      inputComboBuilder.ImmediateStopRecording();
      CloseAllWindows();
      _comboQualityGraph.DrawGraph();
      _graphPanel.SetActive(true);
      Show();
    }

    public void Hide()
    {
      CloseAllWindows();
      _wrap.SetActive(false);
    }

    private void Show()
    {
      _wrap.SetActive(true);
    }

    private void CloseAllWindows()
    {
      _graphPanel.SetActive(false);
    }
  }
}