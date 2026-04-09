using FightDojo;
using UnityEngine;
using UnityEngine.UI;

public class CarriageScroller : MonoBehaviour
{
    [SerializeField] private Carriage _carriage;
    [SerializeField] private InputComboBuilder _inputComboBuilder;
    [SerializeField] private ScrollRect _scrollRect;
    
    private float _smoothSpeed = 0.18f;

    private void LateUpdate()
    {
        if (_inputComboBuilder.IsRecording == false)
            return;
        
        float viewportWidth = _scrollRect.viewport.rect.width;
        float contentWidth  = _scrollRect.content.rect.width;
        if (contentWidth <= viewportWidth) 
            return;
        
        float carriageX = _carriage.Rect.anchoredPosition.x;
        float offset = viewportWidth / 2f;
        float desiredNorm = (carriageX - offset) / (contentWidth - viewportWidth);
        desiredNorm = Mathf.Clamp01(desiredNorm);
        float currentNorm = _scrollRect.horizontalNormalizedPosition;
        _smoothSpeed = 0.18f;
        _scrollRect.horizontalNormalizedPosition =
            Mathf.Lerp(currentNorm, desiredNorm, _smoothSpeed);
    }
}
