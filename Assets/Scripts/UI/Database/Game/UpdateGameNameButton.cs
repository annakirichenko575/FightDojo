using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
    public class UpdateGameNameButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput;

        private GameDataProvider _gameDataProvider;
        private WarningWindow _warningWindow;

        private void Awake()
        {
            _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
            _warningWindow = FindAnyObjectByType<WarningWindow>();
        }

        private void OnEnable()
        {
            _gameDataProvider.CurrentGame(out FightDojo.Database.Game gameData);
            nameInput.text = gameData.Name;
        }

        public void UpdateName()
        {
            string newName = nameInput.text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                _warningWindow.OpenWarning("Новое название не введено!");
                return;
            }

            // 3) обновляем в БД
            _gameDataProvider.UpdateGameName(newName);

            // очистить поля
            nameInput.text = "";
        }
    }
}