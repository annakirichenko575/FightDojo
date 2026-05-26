using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo.UI
{
  public class ButtonUnselector : MonoBehaviour
  {
    public static void Unselect()
    {
      EventSystem.current.SetSelectedGameObject(null);
    }
  }
}
