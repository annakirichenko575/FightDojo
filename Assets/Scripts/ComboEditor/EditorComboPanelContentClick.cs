using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo
{
    // Кликабельный фон/контент панели: двигает каретку, но не является реальным элементом стрипа
    public class EditorComboPanelContentClick : MonoBehaviour, IPointerClickHandler
    {
        // Скрываем поведение StripItemView (которое трогает KeySettingsView и выделение текста)
        public new void OnPointerClick(PointerEventData eventData)
        {
            EditorComboStrip editorCombo = FindFirstObjectByType<EditorComboStrip>();
            editorCombo.MoveCarriage(eventData);
        }

        // На фоне перетаскивание не нужно
        public new void OnDrag(PointerEventData eventData) { }
        public new void OnEndDrag(PointerEventData eventData) { }
    }
}
