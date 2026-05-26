using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FightDojo.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class FloatRangeInputValidator : MonoBehaviour
    {
        [SerializeField] private float _defaultValue = 1f;
        [SerializeField] private float _minValue = 0.25f;
        [SerializeField] private float _maxValue = 1.75f;
        
        private TMP_InputField inputField;
        private string lastValidText;
        
        public event UnityAction<float> OnValidated;
        
        private void Start()
        { 
            inputField = GetComponent<TMP_InputField>();
            
            inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            
            inputField.onValueChanged.AddListener(OnValueChanged);
            inputField.onEndEdit.AddListener(OnEndEdit);

            lastValidText = ToString(_defaultValue);
            inputField.text = lastValidText;
        }

        public float GetValue() => 
            TryParse(inputField.text, out float result) ? result : 0.0f;
        
        private void OnValueChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (TryParse(text, out float value))
            {
                float clamped = Mathf.Clamp(value, _minValue, _maxValue);
                if (ToString(clamped) == lastValidText)
                    return;
                    
                lastValidText = ToString(clamped);
                OnValidated?.Invoke(clamped);
            }
        }

        private void OnEndEdit(string text)
        {
            if (string.IsNullOrEmpty(text) || TryParse(text, out float value) == false)
            {
                inputField.text = lastValidText;
                return;
            }

            float clamped = Mathf.Clamp(value, _minValue, _maxValue);
            inputField.text = ToString(clamped);
            lastValidText = inputField.text;
            
            OnValidated?.Invoke(clamped);
        }

        private string ToString(float value) => 
            value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

        private bool TryParse(string text, out float value) =>
            float.TryParse(text, System.Globalization.NumberStyles.Float, 
                System.Globalization.CultureInfo.InvariantCulture, out value);
    }
}
