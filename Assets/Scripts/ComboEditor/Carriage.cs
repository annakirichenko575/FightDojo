using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo
{
  public class Carriage : MonoBehaviour
  {
    private RectTransform rectCarriage;
    private RectTransform rectContent;

    public void Initialize(RectTransform rectContent)
    {
      this.rectContent = rectContent;
      rectCarriage = GetComponent<RectTransform>();
    }

    public void SetCarriagePosition(PointerEventData eventData)
    {
      Vector2 clickPosition = GetLocalPointerPosition(rectContent, eventData);
      Vector2 position = rectCarriage.anchoredPosition;
      position.x = clickPosition.x;
      rectCarriage.anchoredPosition = position;
    }

    private Vector2 GetLocalPointerPosition(RectTransform rectTransform, PointerEventData eventData)
    {
      Vector2 localPos;

      // Самый важный момент — камера:
      //   • Для Canvas в Screen Space - Overlay → null
      //   • Для Screen Space - Camera → eventData.pressEventCamera (или enterEventCamera для hover)
      //   • Для World Space → тоже eventData.pressEventCamera
      bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
        rectTransform,              // наш RectTransform
        eventData.position,         // экранные координаты указателя (eventData.position)
        eventData.pressEventCamera, // камера (или null для Overlay)
        out localPos
      );
/*
      if (success)
      {
          Debug.Log($"Локальная позиция клика: {localPos}");
          // localPos — это координаты относительно pivot RectTransform
          // (0,0) — в pivot, положительные/отрицательные в зависимости от pivot и anchors
      }
      else
      {
          Debug.LogWarning("Не удалось преобразовать координаты");
      }
      */
      return localPos;
    }

  }
}