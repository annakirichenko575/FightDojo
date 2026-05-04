using System.Collections.Generic;
using FightDojo.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace FightDojo
{
    public class StripItemView : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
    {

        private readonly Dictionary<string, string> ArrowButtonViewMap 
            = new Dictionary<string, string>()
        {
            { "left", "\u2190"},
            { "right", "\u2192"},
            { "up", "\u2191"},
            { "down", "\u2193"},
            
            { "DLeft", "\u2190"},
            { "DRight", "\u2192"},
            { "DUp", "\u2191"},
            { "DDown", "\u2193"},
        };
        
        private readonly Dictionary<string, string> ButtonViewMap 
            = new Dictionary<string, string>()
        {
            { "num0", "0" },
            { "num1", "1" },
            { "num2", "2" },
            { "num3", "3" },
            { "num4", "4" },
            { "num5", "5" },
            { "num6", "6" },
            { "num7", "7" },
            { "num8", "8" },
            { "num9", "9" },
           
            { "padA", "A" },     // A / Cross
            { "padB", "B" },     // B / Circle
            { "padX", "X" },     // X / Square
            { "padY", "Y" },     // Y / Triangle
        };
        
        private readonly Color32 red = new Color32(0xD0, 0x02, 0x05, 0xFF);
        private readonly Color32 redDark = new Color32(139, 0, 0, 0xFF);
        private readonly Color32 white = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        private readonly Color32 gray = new Color32(127, 127, 127, 0xFF);
        private readonly Color32 purple = new Color32(0x97, 0x00, 0xC4, 0xFF);
        private readonly Color32 purpleDark = new Color32(0x33, 0x13, 0x3D, 0xFF);

        [SerializeField] private TMP_FontAsset defaultFont;
        [SerializeField] private TMP_FontAsset secondFont;
        
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
            keyText.text = GetMappedName(keyName);
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

        public void UpdateKey(string keyName)
        { 
            keyText.text = GetMappedName(keyName);
        }
    
        private string GetMappedName(string keyName)
        {
            keyText.font = defaultFont;
            if (ButtonViewMap.TryGetValue(keyName, out string mapped))
            {
                keyName = mapped;
            }
            else if (ArrowButtonViewMap.TryGetValue(keyName, out mapped))
            {
                keyName = mapped;
                keyText.font = secondFont;
            }
            return keyName;
        }

    }
}
