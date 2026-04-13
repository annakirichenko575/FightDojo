using Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.UI;

namespace FightDojo
{
  public class Timeline : MonoBehaviour
  {
    private IAssetProvider _assetProvider;
    private HorizontalLayoutGroup _horizontalGroup;
    private RectTransform _viewRect;
    private RectTransform _contentRect;
    private int _offset;

    public void Initialize(IAssetProvider assetProvider, RectTransform contentRect,
      int offset)
    {
      _assetProvider = assetProvider;
      _offset = offset;
      _contentRect = contentRect;
      _viewRect = _contentRect.parent.GetComponent<RectTransform>();
      _horizontalGroup = GetComponent<HorizontalLayoutGroup>();
      _horizontalGroup.padding.left = _offset;
    }
    
    public void CreateTimeline(float stripScale)
    {
      float minWidth = Mathf.Max(_viewRect.sizeDelta.x, _contentRect.sizeDelta.x);
      float contentWidthInTime = minWidth / stripScale;
      int partsCount = Mathf.CeilToInt(contentWidthInTime);
      Debug.Log(minWidth + " " + stripScale + " " + contentWidthInTime + " " + partsCount);
      float width = 1 * stripScale * partsCount + _offset;
      RectTransform rectTransform = GetComponent<RectTransform>();
      rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
      ClearContent();
      SpawnTimelineParts(partsCount);
    }

    private void SpawnTimelineParts(int partsCount)
    {
      for (int i = 0; i < partsCount; i++)
      {
        GameObject timelinePartGO = _assetProvider.Instantiate(
          AssetPath.TimelinePartPath, transform);
        TimelinePartView timelinePart = timelinePartGO.GetComponent<TimelinePartView>();
        int time = i;
        timelinePart.SetText(time.ToString());
      }
    }

    private void ClearContent()
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
      {
        Object.Destroy(transform.GetChild(i).gameObject);
      }
    }
  }
  
}
