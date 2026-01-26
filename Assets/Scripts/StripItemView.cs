using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace FightDojo
{
    public class StripItemView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        private int id;
        private RectTransform rectTransform;
        private TMP_Text tmpText;

        public int Id => id;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            tmpText = GetComponent<TMP_Text>(); // KeyText (TMP) висит на этом же объекте
        }

        // Вызывается из билдера после Instantiate, чтобы назначить id
        public void Initialize(int id)
        {
            this.id = id;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = rectTransform.anchoredPosition;
            position.x += eventData.delta.x;
            rectTransform.anchoredPosition = position;
            Debug.Log(eventData.delta + " " + rectTransform.anchoredPosition.x);
        }

        // Вызывается Unity, когда кликнули по этому UI-объекту
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked StripItem id={id}");

            KeySettingsView keySettingsView = FindFirstObjectByType<KeySettingsView>();
            keySettingsView.Initialize(this, id);

            EditorComboInitializer editorCombo = FindFirstObjectByType<EditorComboInitializer>();
            editorCombo.Select(this);

            Select();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EditorComboInitializer editorCombo = FindFirstObjectByType<EditorComboInitializer>();
            editorCombo.UpdateTimeByX(id, rectTransform.anchoredPosition.x);
        }

        public void Unselect()
        {
            // делаем букву обычной
            if (tmpText == null) 
                return;

            tmpText.fontStyle &= ~FontStyles.Bold;
        }

        private void Select()
        {
            // делаем букву жирным
            if (tmpText == null) 
                return;

            tmpText.fontStyle |= FontStyles.Bold;
        }
    }
}
