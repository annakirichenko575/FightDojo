using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
    public class AddGameButton : MonoBehaviour
    {
        public TMP_InputField nameInput;
    
        private GameDataProvider gameDataProvider;
        private WarningWindow warningWindow;

        private void Awake()
        {
            gameDataProvider = FindAnyObjectByType<GameDataProvider>();
            warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        public void AddGame()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                warningWindow.OpenWarning("Новое название не введено!");
                return;
            }
        
            gameDataProvider.AddGame(newName);
            nameInput.text = "";
        }
    }
}
