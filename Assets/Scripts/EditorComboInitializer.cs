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

        public void BuildStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.BuildComboStrip(recordedKeys);
        }

        public void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }

        public KeyData FindKey(int id)
        {
            return recordedKeys.GetEditorStripItem(id);
        }

        //удалить элемент из данных
        public void Delete(int id)
        {
            recordedKeys.Delete(id);
        }
    }
}
