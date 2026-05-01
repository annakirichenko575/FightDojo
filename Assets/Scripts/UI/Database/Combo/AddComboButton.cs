using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
    public class AddComboButton : MonoBehaviour
    {
        public TMP_InputField nameInput;
        public TMP_InputField descriptionInput;
        public TMP_InputField tagsInput;
    
        private ComboDataProvider _comboDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            //nameInput.text = "";
        }

        public void AddCombo()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое имя автора не введено!");
                return;
            }
        
            _comboDataProvider.AddCombo(newName, descriptionInput.text.Trim(), tagsInput.text.Trim());
            nameInput.text = "";
            descriptionInput.text = "";
            tagsInput.text = "";
        }
    }
}