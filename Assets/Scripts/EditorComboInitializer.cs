using System;
using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float stripScale = 2000f;

        private RecordedKeys recordedKeys;
        private EditorComboBuilder comboBuilder;
        private StripItemView currentStripItemView = null;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
            comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.Initialize(offset, stripScale);

            BuildStrip();
        }

        public void Update()
        {
            //считать нажатие кнопки и кнопку keyData по id из  currentStripItemView
        }

        public void BuildStrip()
        {
            ClearStrip();
            comboBuilder.BuildComboStrip(recordedKeys);
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

        public void UpdateTimeByX(int id, float x)
        {
            KeyData keyData = FindKey(id);
            keyData.Time = (x - offset.x) / stripScale;
            BuildStrip();
        }

        public void Select(StripItemView stripItemView)
        {
            if (currentStripItemView != null)
            {
                currentStripItemView.Unselect();
            }
            currentStripItemView = stripItemView;
        }

        private void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }

    }
}
