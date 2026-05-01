using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
    public class AddGameButton : MonoBehaviour
    {
        public TMP_InputField nameInput;
    
        private GameDataProvider _gameDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            //nameInput.text = "";
        }

        public void AddGame()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое название не введено!");
                return;
            }
        
            _gameDataProvider.AddGame(newName);
            nameInput.text = "";
        }
    }
}
