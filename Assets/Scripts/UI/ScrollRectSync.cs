using UnityEngine;
using UnityEngine.UI;

namespace FightDojo.UI
{
  public class ScrollRectSync : MonoBehaviour
  {
    public ScrollRect editorScroll;
    public ScrollRect inputScroll;

    private bool isSyncing = false;

    private void OnEnable()
    {
      editorScroll.onValueChanged.AddListener(OnEditorScrollChanged);
      inputScroll.onValueChanged.AddListener(OnInputScrollChanged);
    }

    private void OnDisable()
    {
      editorScroll.onValueChanged.RemoveListener(OnEditorScrollChanged);
      inputScroll.onValueChanged.RemoveListener(OnInputScrollChanged);
    }

    private void OnEditorScrollChanged(Vector2 value)
    {
      if (isSyncing) 
        return;
        
      isSyncing = true; 
      inputScroll.horizontalNormalizedPosition = value.x;
      isSyncing = false;
    }

    private void OnInputScrollChanged(Vector2 value)
    {
      if (isSyncing) 
        return;
        
      isSyncing = true;
      editorScroll.horizontalNormalizedPosition = value.x;
      isSyncing = false;
    }}
}
