using UnityEngine;

namespace FightDojo.UI
{
  [RequireComponent(typeof(FloatRangeInputValidator))]
  public class TimeSpeedInput : MonoBehaviour
  {
    private FloatRangeInputValidator _validator;
    private InputComboBuilder _inputComboBuilder;
    
    private void Start()
    {
      _inputComboBuilder = FindAnyObjectByType<InputComboBuilder>();
      _validator = GetComponent<FloatRangeInputValidator>();
      _validator.OnValidated += SpeedChanged;
    }

    private void SpeedChanged(float speed) => 
      _inputComboBuilder.SpeedChanged(speed);
  }
}
