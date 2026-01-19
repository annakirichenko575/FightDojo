using System.Linq;
using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        private RecordedKeys recordedKeys;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
            BuildStrip();
        }

        // Собирает полоску из recordData
        public void BuildStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.BuildComboStrip(recordedKeys);
        }

        // Очищает текущую полоску (контент)
        public void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }

        // Находит нужный RecordedEvent по id
        public KeyData FindKey(int id)
        {
            return recordedKeys.GetEditorStripItem(id);
        }
    }
}
