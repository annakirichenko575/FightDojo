using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo.UI.Focus
{
  public class FocusPanel : MonoBehaviour, IPointerClickHandler
  {
    public bool IsFocused { get; private set; } = true;
  
    public void OnPointerClick(PointerEventData eventData)
    {
      Focus();
    }

    public void LoseFocus()
    {
      IsFocused = false;
    }

    public void Focus()
    {
      IsFocused = true;
    }
  }
}
