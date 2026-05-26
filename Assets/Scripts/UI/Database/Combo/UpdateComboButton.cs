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
    
        private ComboDataProvider comboDataProvider;
        private WarningWindow warningWindow;

        private void Awake()
        {
            comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
            warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            comboDataProvider.CurrentCombo(out Combos comboData);
            nameInput.text = comboData.CreatorName;
            descriptionInput.text = comboData.Description;
            tagsInput.text = comboData.Tags;
        }

        public void UpdateCombo()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                warningWindow.OpenWarning("Новое имя автора не введено!");
                return;
            }

            comboDataProvider.UpdateCombo(newName, descriptionInput.text.Trim(), tagsInput.text.Trim());
            nameInput.text = "";
            descriptionInput.text = "";
            tagsInput.text = "";
        }
    }
}