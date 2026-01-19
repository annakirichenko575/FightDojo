using System;
using System.Globalization;
using FightDojo.Data.AutoKeyboard;
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

        private RecordedEvent recordedEvent = null;

        public void Initialize(StripItemView stripItemView, int id)
        {
            recordedEvent = editorCombo.FindKey(id);
            inputKey.text = recordedEvent.key_name_display;
            inputTime.text = recordedEvent.delay_ms.ToString(CultureInfo.InvariantCulture);

            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(Apply);
        }

        private void Apply()
        {
            if (recordedEvent == null)
                return;

            //данные из инпутов в recordedEvent
            recordedEvent.key_name_display = inputKey.text;

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
            recordedEvent.delay_ms = ms;

            //зачистить стрип EditorComboBuilder-ом через EditorComboInitializer
            editorCombo.ClearStrip();

            //пересобрать стрип через EditorComboInitializer
            editorCombo.BuildStrip();
        }
    }
}
