using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float stripScale = 2000f;

        private RecordedKeys recordedKeys;
        private EditorComboBuilder comboBuilder;
        private StripItemView currentStripItemView = null;
        private KeyInputReader keyInputReader = new KeyInputReader();        

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
            // Если ничего не выбрано — нечего менять
            if (currentStripItemView == null)
                return;

            if (Keyboard.current == null)
                return;

            KeyData inputKeyData = keyInputReader.CheckKeys();

            if (inputKeyData == null || inputKeyData.Action != KeyData.IsPressedAction)
                return;

            // Берём KeyData выбранного элемента и меняем KeyName
            recordedKeys.UpdateKeyName(currentStripItemView.Id, inputKeyData.KeyName);

            // Сразу обновим текст на выбранном объекте, без пересборки стрипа
            TMP_Text txt = currentStripItemView.GetComponent<TMP_Text>();
            if (txt != null)
                txt.text = inputKeyData.KeyName;
        }

        // Возвращает имя первой нажатой клавиши из списка (или null)
        private string GetPressedKeyName(Key[] keys)
        {
            foreach (var k in keys)
            {
                var key = Keyboard.current[k];
                if (key != null && key.wasPressedThisFrame)
                    return key.keyCode.ToString();
            }
            return null;
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

        // удалить элемент из данных
        public void Delete(int id)
        {
            recordedKeys.Delete(id);
        }

        public void UpdateTimeByX(int id, float x)
        {
            recordedKeys.UpdateKeyTime(id, (x - offset.x) / stripScale);
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
