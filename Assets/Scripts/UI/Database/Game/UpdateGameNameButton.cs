using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
  public class UpdateGameNameButton : MonoBehaviour
  {
    [SerializeField] private TMP_InputField nameInput;

    private GameDataProvider gameDataProvider;
    private WarningWindow warningWindow;

    private void Awake()
    {
      gameDataProvider = FindAnyObjectByType<GameDataProvider>();
      warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    private void OnEnable()
    {
      gameDataProvider.CurrentGame(out FightDojo.Database.Game gameData);
      nameInput.text = gameData.Name;
    }

    public void UpdateName()
    {
      string newName = nameInput.text.Trim();
      if (string.IsNullOrWhiteSpace(newName))
      {
        warningWindow.OpenWarning("Новое название не введено!");
        return;
      }

      // 3) обновляем в БД
      gameDataProvider.UpdateGameName(newName);

      // очистить поля
      nameInput.text = "";
    }
  }
}