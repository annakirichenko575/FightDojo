using System.Globalization;
using FightDojo.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FightDojo
{
    public class KeySettingsView : MonoBehaviour
    {
        [SerializeField] private EditorComboInitializer editorCombo;
        [SerializeField] private TMP_InputField inputKey;
        [SerializeField] private TMP_InputField inputTime;
        [SerializeField] private Button okButton;

        // NEW: кнопка удаления рядом с OK
        [SerializeField] private Button deleteButton;

        private KeyData keyData = null;
        private int currentId = -1;

        public void Initialize(StripItemView stripItemView, int id)
        {
            currentId = id;

            keyData = editorCombo.FindKey(id);
            inputKey.text = keyData.KeyName;
            inputTime.text = keyData.Time.ToString(CultureInfo.InvariantCulture);

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(Apply);

            if (deleteButton != null)
            {
                deleteButton.onClick.RemoveAllListeners();
                deleteButton.onClick.AddListener(Delete);
            }
        }

        private void Apply()
        {
            if (keyData == null)
                return;

            float time;
            if (!float.TryParse(inputTime.text, NumberStyles.Float, CultureInfo.InvariantCulture, out time))
            {
                if (!float.TryParse(inputTime.text, NumberStyles.Float, CultureInfo.CurrentCulture, out time))
                {
                    Debug.LogWarning($"Не могу распарсить время: '{inputTime.text}'");
                    return;
                }
            }

            // данные из инпутов в KeyData
            keyData.Set(keyData.Action, time, inputKey.text);

            // зачистить и пересобрать
            editorCombo.BuildStrip();
        }

        // NEW: удаление выбранного элемента
        private void Delete()
        {
            if (currentId < 0)
                return;

            editorCombo.Delete(currentId);

            editorCombo.BuildStrip();
        }
    }
}
