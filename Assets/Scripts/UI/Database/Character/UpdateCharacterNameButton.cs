using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Character
{
    public class UpdateCharacterNameButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;

        private CharacterDataProvider _characterDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            _characterDataProvider.CurrentCharacter(out FightDojo.Database.Character characterData);
            nameInput.text = characterData.Name;
        }

        public void UpdateName()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое название не введено!");
                return;
            }

            _characterDataProvider.UpdateCharacterName(newName);
            nameInput.text = "";
        }
    }
}