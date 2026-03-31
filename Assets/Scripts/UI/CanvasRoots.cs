using FightDojo;
using FightDojo.Data;
using FightDojo.Database;
using Services;
using UnityEngine;

public class CanvasRoots : MonoBehaviour
{
  [SerializeField] private GameObject _dbCanvas;
  [SerializeField] private GameObject _comboCanvas;
  [SerializeField] private ComboDataProvider _comboProvider;
  [SerializeField] private EditorComboStrip _editorComboStrip;

  public void Start()
  {
    OpenDbCanvas();
  }
  
  public void OpenDbCanvas()
  {
    _comboCanvas.SetActive(false);
    _dbCanvas.SetActive(true);
  }

  public void OpenComboCanvas()
  {
    _comboProvider.CurrentCombo(out Combos combos);
    Debug.Log(combos);
    IRecordedKeysService recordedKeys = 
      AllServices.Container.Single<IRecordedKeysService>();
    recordedKeys.LoadJson(combos.Combo);
    _editorComboStrip.Open();
    _dbCanvas.SetActive(false);
    _comboCanvas.SetActive(true);
  }
}
