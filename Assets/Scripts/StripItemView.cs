using UnityEngine;
using UnityEngine.EventSystems;

namespace FightDojo
{
    public class StripItemView : MonoBehaviour, IPointerClickHandler
    {
        private int id;

        // Вызывается из билдера после Instantiate, чтобы назначить id
        public void Initialize(int id)
        {
            this.id = id;
        }

        // Вызывается Unity, когда кликнули по этому UI-объекту
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked StripItem id={id}");
            KeySettingsView keySettingsView = FindFirstObjectByType<KeySettingsView>();
            keySettingsView.Initialize(this, id);
        }
    }
}
