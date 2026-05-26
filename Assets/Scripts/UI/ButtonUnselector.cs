using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUnselector : MonoBehaviour
{
  public static void Unselect()
  {
    EventSystem.current.SetSelectedGameObject(null);
  }
}
