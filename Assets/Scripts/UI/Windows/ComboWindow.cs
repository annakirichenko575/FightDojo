using UnityEngine;

namespace FightDojo.UI.Windows
{
    public class ComboWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _wrap;
        [SerializeField] private GameObject _addWindow;
        [SerializeField] private GameObject _updateWindow;
        [SerializeField] private GameObject _deleteWindow;
        private ComboDataProvider _comboDataProvider;

        private void Awake()
        {
            _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
        }

        private void Start()
        {
            Hide();
        }

        public void OpenAddWindow()
        {
            CloseAllWindows();
            _addWindow.SetActive(true);
            Show();
        }

        public void OpenUpdateWindow()
        {
            if (_comboDataProvider.HasSelectedCombo == false)
                return;
                
            CloseAllWindows();
            _updateWindow.SetActive(true);
            Show();
        }

        public void OpenDeleteWindow()
        {
            if (_comboDataProvider.HasSelectedCombo == false)
                return;
                
            CloseAllWindows();
            _deleteWindow.SetActive(true);
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
            _deleteWindow.SetActive(false);
            _addWindow.SetActive(false);
            _updateWindow.SetActive(false);
        }
    }
}