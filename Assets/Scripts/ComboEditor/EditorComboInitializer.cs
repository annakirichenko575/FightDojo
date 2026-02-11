using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float stripScale = 2000f;
        [SerializeField] private Transform carriage;

        private RecordedKeys recordedKeys;
        private EditorComboBuilder comboBuilder;
        private StripItemView currentStripItemView = null;
        private KeyInputReader keyInputReader = new KeyInputReader();        

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
            comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.Initialize(offset, stripScale, carriage);

            BuildStrip();
        }

        public void Update()
        {
            // Если ничего не выбрано — нечего менять
            if (currentStripItemView == null)
                return;
            
            if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)
            {
                Delete(currentStripItemView.Id);
                Destroy(currentStripItemView.gameObject);
                currentStripItemView = null;
                return;
            }

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

        public void Select(StripItemView stripItemView, PointerEventData eventData)
        {
            SetCarriagePosition(eventData);

            if (currentStripItemView != null)
            {
                currentStripItemView.Unselect();
            }
            currentStripItemView = stripItemView;
        }

        private void SetCarriagePosition(PointerEventData eventData)  //В класс каретки
        {
            RectTransform rectContent = carriage.GetComponent<RectTransform>();
            Vector2 clickPosition = GetLocalPointerPosition(rectContent, eventData);
            Vector2 position = rectContent.anchoredPosition;
            position.x = clickPosition.x;
            rectContent.anchoredPosition = position;
        }

        private void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }
    
        private Vector2 GetLocalPointerPosition(RectTransform rectTransform, PointerEventData eventData)  //В класс каретки
        {
            Vector2 localPos;

            // Самый важный момент — камера:
            //   • Для Canvas в Screen Space - Overlay → null
            //   • Для Screen Space - Camera → eventData.pressEventCamera (или enterEventCamera для hover)
            //   • Для World Space → тоже eventData.pressEventCamera
            bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,              // наш RectTransform
                eventData.position,         // экранные координаты указателя (eventData.position)
                eventData.pressEventCamera, // камера (или null для Overlay)
                out localPos
            );

/*
            if (success)
            {
                Debug.Log($"Локальная позиция клика: {localPos}");
                // localPos — это координаты относительно pivot RectTransform
                // (0,0) — в pivot, положительные/отрицательные в зависимости от pivot и anchors
            }
            else
            {
                Debug.LogWarning("Не удалось преобразовать координаты");
            }
            */
            return localPos;
        }
    }
}
