using FightDojo.Data;
using Services;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
    public class SaveComboJsonButton : MonoBehaviour
    {
        private ComboDataProvider _comboDataProvider;
        private IRecordedKeysService _recordedKeys;
        private EditorComboStrip _editorComboStrip;

        private void Awake()
        {
            _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
            _editorComboStrip = FindAnyObjectByType<EditorComboStrip>();
            _recordedKeys = AllServices.Container.Single<IRecordedKeysService>();
        }

        public void SaveCombo()
        {
            string json = _recordedKeys.ToJson();
            _comboDataProvider.UpdateComboJson(json);
            _editorComboStrip.ResetChangeFlag();
            Debug.Log(json);
        }
    }
}