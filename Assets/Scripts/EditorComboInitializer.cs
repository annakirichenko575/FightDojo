using System.Linq;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        private RecordData recordData; 

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordData = jsonLoader.Load();
            BuildStrip();
        }

        // Собирает полоску из recordData
        public void BuildStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.BuildComboStrip(recordData);
        }

        // Очищает текущую полоску (контент)
        public void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }

        // Находит нужный RecordedEvent по id
        public RecordedEvent FindKey(int id)
        {
            return recordData.recorded_events_v2.First(item => item.id == id);
        }
    }
}
