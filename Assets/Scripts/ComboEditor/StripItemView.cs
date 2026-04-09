using System;
using FightDojo.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace FightDojo
{
    public class StripItemView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        private readonly Color32 red = new Color32(208, 2, 5, 0xFF);
        private readonly Color32 redDark = new Color32(139, 0, 0, 0xFF);
        private readonly Color32 white = new Color32(255, 255, 255, 0xFF);
        private readonly Color32 gray = new Color32(127, 127, 127, 0xFF);
        private readonly Color32 purple = new Color32(151, 0, 96, 0xFF);
        private readonly Color32 purpleDark = new Color32(76, 1, 99, 0xFF);
            
        private int id;
        private RectTransform rectTransform;
        private TMP_Text keyText;
        private string action;
        private bool isInput;

        public int Id => id;

        public void Initialize(int id, Vector2 timeOffset, 
            string keyName, string action, bool isInput)
        {
            this.isInput = isInput;
            rectTransform = GetComponent<RectTransform>();
            keyText = GetComponent<TMP_Text>(); // KeyText (TMP) висит на этом же объекте
            this.id = id;
            rectTransform.anchoredPosition = timeOffset;
            keyText.text = keyName;
            this.action = action;
            SetColor(white, gray);
        }
        
        public void SetErrorColor()
        {
            SetColor(red, redDark);
        }

        private void SetColor(Color PressColor, Color ReleaseColor)
        {
            keyText.color = (action == KeyData.IsPressedAction)
                ? new Color(PressColor.r, PressColor.g, PressColor.b)
                : new Color(ReleaseColor.r, ReleaseColor.g, ReleaseColor.b);
        }

        private void Update()
        {
            if (isInput)
                Debug.Log(keyText.color);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isInput)
                return;
            
            Vector2 position = rectTransform.anchoredPosition;
            position.x += eventData.delta.x;
            rectTransform.anchoredPosition = position;
            Debug.Log(eventData.delta + " " + rectTransform.anchoredPosition.x);
        }

        // Вызывается Unity, когда кликнули по этому UI-объекту
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isInput)
                return;
            
            Debug.Log($"Clicked StripItem id={id}");

            /*KeySettingsView keySettingsView = FindFirstObjectByType<KeySettingsView>();
            keySettingsView.Initialize(this, id);*/

            EditorComboStrip editorCombo = FindFirstObjectByType<EditorComboStrip>();
            editorCombo.SelectKey(this, eventData); //Это перемещает карретку
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isInput)
                return;
            
            EditorComboStrip editorCombo = FindFirstObjectByType<EditorComboStrip>();
            editorCombo.UpdateTimeByX(id, rectTransform.anchoredPosition.x);
        }

        public void Unselect()
        {
            Debug.Log("Unselect" + Id);
            // делаем букву обычной
            if (keyText == null) 
                return;

            keyText.fontStyle &= ~FontStyles.Bold;
        }

        public void Select()
        {
            Debug.Log("Select" + Id);
            // делаем букву жирным
            if (keyText == null) 
                return;
            
            keyText.fontStyle |= FontStyles.Bold;
        }

        
    }
}
