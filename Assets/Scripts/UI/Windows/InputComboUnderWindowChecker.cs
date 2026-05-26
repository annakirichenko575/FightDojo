using UnityEngine;

namespace FightDojo.UI.Windows
{
  public class InputComboUnderWindowChecker : MonoBehaviour
  {
    [SerializeField] private GameObject _comboWindowWrap;
    [SerializeField] private GameObject _warningWindowWrap;
    [SerializeField] private GameObject _graphWindowWrap;

    public bool IsOpened => _comboWindowWrap.activeSelf
                            || _warningWindowWrap.activeSelf
                            || _graphWindowWrap.activeSelf;
  }
}