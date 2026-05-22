using UnityEngine;
using UnityEngine.UI;

namespace FightDojo.UI
{
    public class CarriageScroller : MonoBehaviour
    {
        [SerializeField] private Carriage _carriage;
        [SerializeField] private ScrollRect _scrollRectEditor;
        [SerializeField] private ScrollRect _scrollRectInput;
    
        private float _smoothSpeed = 0.18f;

        public void AtStartPosition()
        {
            _scrollRectInput.horizontalNormalizedPosition = 0f;
            _scrollRectEditor.horizontalNormalizedPosition = 0f;
        }
        
        public void Scroll()
        {
            float viewportWidth = _scrollRectInput.viewport.rect.width;
            float contentWidth  = _scrollRectInput.content.rect.width;
            if (contentWidth <= viewportWidth) 
                return;
        
            float carriageX = _carriage.Rect.anchoredPosition.x;
            float offset = viewportWidth / 2f;
            float desiredNorm = (carriageX - offset) / (contentWidth - viewportWidth);
            desiredNorm = Mathf.Clamp01(desiredNorm);
            float currentNorm = _scrollRectInput.horizontalNormalizedPosition;
            _smoothSpeed = 0.18f;
            _scrollRectInput.horizontalNormalizedPosition =
                //Mathf.Lerp(currentNorm, desiredNorm, _smoothSpeed);
                desiredNorm;
        }
    }
}
