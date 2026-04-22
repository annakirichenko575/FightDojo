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
        
        private TMP_InputField _inputField;
        private string _lastValidText;
        
        public event UnityAction<float> OnValidated;
        
        private void Start()
        { 
            _inputField = GetComponent<TMP_InputField>();
            
            _inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            _inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
            
            _inputField.onValueChanged.AddListener(OnValueChanged);
            _inputField.onEndEdit.AddListener(OnEndEdit);

            _lastValidText = ToString(_defaultValue);
            _inputField.text = _lastValidText;
        }

        public float GetValue() => 
            TryParse(_inputField.text, out float result) ? result : 0.0f;
        
        private void OnValueChanged(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (TryParse(text, out float value))
            {
                float clamped = Mathf.Clamp(value, _minValue, _maxValue);
                if (ToString(clamped) == _lastValidText)
                    return;
                    
                _lastValidText = ToString(clamped);
                OnValidated?.Invoke(clamped);
            }
        }

        private void OnEndEdit(string text)
        {
            if (string.IsNullOrEmpty(text) || TryParse(text, out float value) == false)
            {
                _inputField.text = _lastValidText;
                return;
            }

            float clamped = Mathf.Clamp(value, _minValue, _maxValue);
            _inputField.text = ToString(clamped);
            _lastValidText = _inputField.text;
            
            OnValidated?.Invoke(clamped);
        }

        private string ToString(float value) => 
            value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

        private bool TryParse(string text, out float value) =>
            float.TryParse(text, System.Globalization.NumberStyles.Float, 
                System.Globalization.CultureInfo.InvariantCulture, out value);
    }
}
