using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Character
{
    public class AddCharacterButton : MonoBehaviour
    {
        public TMP_InputField nameInput;
    
        private CharacterDataProvider _characterDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            //nameInput.text = "";
        }

        public void AddCharacter()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое название не введено!");
                return;
            }
        
            _characterDataProvider.AddCharacter(newName);
            nameInput.text = "";
        }
    }
}