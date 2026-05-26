using FightDojo.ComboEditor;
using FightDojo.Database;
using FightDojo.ComboEditor.Data;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
  public class SaveComboJsonButton : MonoBehaviour
  {
    private ComboDataProvider comboDataProvider;
    private IRecordedKeysService recordedKeys;
    private EditorComboStrip editorComboStrip;

    private void Awake()
    {
      comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
      editorComboStrip = FindAnyObjectByType<EditorComboStrip>();
      recordedKeys = AllServices.Container.Single<IRecordedKeysService>();
    }

    public void SaveCombo()
    {
      string json = recordedKeys.ToJson();
      comboDataProvider.UpdateComboJson(json);
      editorComboStrip.ResetChangeFlag();
      Debug.Log(json);
    }
  }
}