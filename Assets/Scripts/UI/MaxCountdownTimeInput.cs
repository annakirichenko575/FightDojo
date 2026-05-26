using FightDojo.ComboEditor;
using UnityEngine;

namespace FightDojo.UI
{
  [RequireComponent(typeof(FloatRangeInputValidator))]
  public class MaxCountdownTimeInput : MonoBehaviour
  {
    private FloatRangeInputValidator validator;
    private InputComboBuilder inputComboBuilder;
    
    private void Start()
    {
      inputComboBuilder = FindAnyObjectByType<InputComboBuilder>();
      validator = GetComponent<FloatRangeInputValidator>();
      validator.OnValidated += MaxTimeChanged;
    }

    private void MaxTimeChanged(float maxTime) => 
      inputComboBuilder.MaxCountdownTimeChanged(maxTime);
  }
}
