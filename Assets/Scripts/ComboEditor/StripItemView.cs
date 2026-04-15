using FightDojo.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace FightDojo
{
    public class StripItemView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        private readonly Color32 red = new Color32(0xD0, 0x02, 0x05, 0xFF);
        private readonly Color32 redDark = new Color32(139, 0, 0, 0xFF);
        private readonly Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        private readonly Color32 gray = new Color32(127, 127, 127, 0xFF);
        private readonly Color32 purple = new Color32(0x97, 0x00, 0xC4, 0xFF);
        private readonly Color32 purpleDark = new Color32(0x33, 0x13, 0x3D, 0xFF);
            
        private int id;
        private RectTransform rectTransform;
        private TMP_Text keyText;
        private string action;
        private bool isInput;
        private float time;
        private KeyTextSpawner keyTextSpawner;

        public int Id => id;

        public void Initialize(int id, string keyName, float time, string action, 
            KeyTextSpawner keyTextSpawner, bool isInput)
        {
            this.id = id;
            this.time = time;
            this.action = action;
            this.keyTextSpawner = keyTextSpawner;
            this.isInput = isInput;
            rectTransform = GetComponent<RectTransform>();
            keyText = GetComponent<TMP_Text>(); // KeyText (TMP) висит на этом же объекте
            rectTransform.anchoredPosition = keyTextSpawner.GetTimeOffset(time);
            keyText.text = keyName;
            SetColor(white, gray);
        }
        
        public void ChangeScale()
        {
            rectTransform.anchoredPosition = keyTextSpawner.GetTimeOffset(time);
        }
        
        public void SetCorrectColor() => 
            SetColor(purple, purpleDark);

        public void SetErrorColor() => 
            SetColor(red, redDark);

        private void SetColor(Color PressColor, Color ReleaseColor)
        {
            keyText.color = (action == KeyData.IsPressedAction)
                ? new Color(PressColor.r, PressColor.g, PressColor.b, 0.5f)
                : new Color(ReleaseColor.r, ReleaseColor.g, ReleaseColor.b, 0.5f);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isInput)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Left)
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
            
            if (eventData.button != PointerEventData.InputButton.Left)
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
            
            if (eventData.button != PointerEventData.InputButton.Left)
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
            // делаем букву жирным
            if (keyText == null) 
                return;
            
            keyText.fontStyle |= FontStyles.Bold;
        }

    }
}
