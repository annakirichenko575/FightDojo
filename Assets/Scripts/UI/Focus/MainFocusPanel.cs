using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo.UI.Focus
{
  public class MainFocusPanel : MonoBehaviour
  {
    [SerializeField] public FocusPanel focusPanel;

    private void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        if (IsClickInside(focusPanel))
        {
          focusPanel.Focus();
        }
        else
        {
          focusPanel.LoseFocus();
        }
      }
    }

    private bool IsClickInside(FocusPanel panel)
    {
      PointerEventData eventData = new PointerEventData(EventSystem.current);
      eventData.position = Input.mousePosition;
    
      List<RaycastResult> results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(eventData, results);

      foreach (var r in results)
      {
        if (r.gameObject.transform.IsChildOf(panel.transform))
          return true;
      }

      return false;
    }
  }
}
