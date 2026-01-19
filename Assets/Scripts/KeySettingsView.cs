using System;
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

        private KeyData keyData = null;

        public void Initialize(StripItemView stripItemView, int id)
        {
            keyData = editorCombo.FindKey(id);
            inputKey.text = keyData.KeyName;
            inputTime.text = keyData.Time.ToString(CultureInfo.InvariantCulture);

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(Apply);
        }

        private void Apply()
        {
            if (keyData == null)
                return;

            float ms;
            if (!float.TryParse(inputTime.text, NumberStyles.Float, CultureInfo.InvariantCulture, out ms))
            {
                // если юзер ввёл через запятую, пробуем текущую культуру
                if (!float.TryParse(inputTime.text, NumberStyles.Float, CultureInfo.CurrentCulture, out ms))
                {
                    Debug.LogWarning($"Не могу распарсить время: '{inputTime.text}'");
                    return;
                }
            }
            //данные из инпутов в recordedEvent
            keyData.Set(keyData.Action, ms, inputKey.text);

            //зачистить стрип EditorComboBuilder-ом через EditorComboInitializer
            editorCombo.ClearStrip();

            //пересобрать стрип через EditorComboInitializer
            editorCombo.BuildStrip();
        }

        /*
        Delete
        {
            editorCombo.Dlete(id)
        }
        */
    }
}
