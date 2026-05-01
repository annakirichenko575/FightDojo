using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
    public class UpdateComboButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private TMP_InputField tagsInput;
    
        private ComboDataProvider _comboDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            _comboDataProvider.CurrentCombo(out Combos comboData);
            nameInput.text = comboData.CreatorName;
            descriptionInput.text = comboData.Description;
            tagsInput.text = comboData.Tags;
        }

        public void UpdateCombo()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое имя автора не введено!");
                return;
            }

            _comboDataProvider.UpdateCombo(newName, descriptionInput.text.Trim(), tagsInput.text.Trim());
            nameInput.text = "";
            descriptionInput.text = "";
            tagsInput.text = "";
        }
    }
}