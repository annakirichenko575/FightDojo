using System;
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
            inputTime.text = recordedEvent.delay_ms.ToString();
            okButton.onClick.RemoveAllListeners();
            okButton.onClick.AddListener(Apply);
        }

        private void Apply()
        {
            if (recordedEvent == null)
                return;
                
            //забрать данные из инпутов в recordedEvent
            //зачистить EditorCombo билдером стрип обращаясь к нему через EditorComboInitializer
            //выполнить "BuildStrip" у EditorComboInitializer
        }
    }
}
