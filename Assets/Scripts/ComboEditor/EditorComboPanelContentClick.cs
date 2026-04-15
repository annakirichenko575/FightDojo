using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo
{
    // Кликабельный фон/контент панели: двигает каретку, но не является реальным элементом стрипа
    public class EditorComboPanelContentClick : MonoBehaviour, 
        IPointerClickHandler, IScrollHandler
    {
        private EditorComboStrip editorStrip;
        private EditorComboStrip EditorStrip => 
            editorStrip != null ? editorStrip : 
                editorStrip = FindFirstObjectByType<EditorComboStrip>();    

        // Скрываем поведение StripItemView (которое трогает KeySettingsView и выделение текста)
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            EditorStrip.MoveCarriage(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            float sign = Mathf.Sign(eventData.scrollDelta.y);
            EditorStrip.ChangeScale(sign);
        }

    }
    
}
