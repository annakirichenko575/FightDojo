using UnityEngine;

namespace FightDojo.UI
{
  [RequireComponent(typeof(FloatRangeInputValidator))]
  public class MaxCountdownTimeInput : MonoBehaviour
  {
    private FloatRangeInputValidator _validator;
    private InputComboBuilder _inputComboBuilder;
    
    private void Start()
    {
      _inputComboBuilder = FindAnyObjectByType<InputComboBuilder>();
      _validator = GetComponent<FloatRangeInputValidator>();
      _validator.OnValidated += MaxTimeChanged;
    }

    private void MaxTimeChanged(float maxTime) => 
      _inputComboBuilder.MaxCountdownTimeChanged(maxTime);
  }
}
