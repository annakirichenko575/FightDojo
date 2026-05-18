using UnityEngine;

namespace FightDojo.UI.Windows.SaveWarning
{
    public class SaveWarningWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _wrap;
        [SerializeField] private GameObject _waningPanel;

        private void Start()
        {
            Hide();
        }
        
        public void Open()
        {
            CloseAllWindows();
            _waningPanel.SetActive(true);
            Show();
        }

        public void Hide()
        {
            CloseAllWindows();
            _wrap.SetActive(false);
        }
        
        private void Show()
        {
            _wrap.SetActive(true);
        }

        private void CloseAllWindows()
        {
            _waningPanel.SetActive(false);
        }
    }
}
