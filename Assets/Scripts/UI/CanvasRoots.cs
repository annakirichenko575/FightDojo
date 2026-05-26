using FightDojo.ComboEditor;
using FightDojo.ComboEditor.Data;
using FightDojo.Database;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.UI
{
  public class CanvasRoots : MonoBehaviour
  {
    [SerializeField] private GameObject _dbCanvas;
    [SerializeField] private GameObject _comboCanvas;
    [SerializeField] private ComboDataProvider _comboProvider;
    [SerializeField] private CharacterDataProvider _characterProvider;
    [SerializeField] private GameDataProvider _gameProvider;
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
      _characterProvider.CurrentCharacter(out Character character);
      _gameProvider.CurrentGame(out Game game);
      Debug.Log(combos);
      IRecordedKeysService recordedKeys = 
        AllServices.Container.Single<IRecordedKeysService>();
      recordedKeys.LoadJson(combos.Combo);
      ICurrentComboInfoService currentComboInfo = AllServices.Container.Single<ICurrentComboInfoService>();
      currentComboInfo.UpdateComboInfo(combos.Id, combos.CreatorName, character.Name, game.Name);
      _editorComboStrip.Open();
      _dbCanvas.SetActive(false);
      _comboCanvas.SetActive(true);
    }
  }
}
