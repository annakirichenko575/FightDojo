using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo
{
    // Кликабельный фон/контент панели: двигает каретку, но не является реальным элементом стрипа
    public class EditorComboPanelContentClick : StripItemView, IPointerClickHandler
    {
        private const int BackgroundId = -1;

        private void Start()
        {
            // Чтобы этот "псевдо-элемент" не совпал по id с реальными
            Initialize(BackgroundId);
        }

        // Скрываем поведение StripItemView (которое трогает KeySettingsView и выделение текста)
        public new void OnPointerClick(PointerEventData eventData)
        {
            EditorComboStrip editorCombo = FindFirstObjectByType<EditorComboStrip>();
            if (editorCombo == null)
                return;

            editorCombo.Select(this, eventData);
        }

        // На фоне перетаскивание не нужно
        public new void OnDrag(PointerEventData eventData) { }
        public new void OnEndDrag(PointerEventData eventData) { }
    }
}
