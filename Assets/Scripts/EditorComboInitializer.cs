using System.Linq;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;

namespace FightDojo
{

    public class EditorComboInitializer : MonoBehaviour
    {
        private RecordData recordData; //Превратить в отдельную сущность

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordData = jsonLoader.Load();
            BuildStrip();
        }

        public void BuildStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.BuildComboStrip(recordData);
        }

        public RecordedEvent FindKey(int id)
        {
            return recordData.recorded_events_v2.First(item => item.id == id);
        }

    }
}
