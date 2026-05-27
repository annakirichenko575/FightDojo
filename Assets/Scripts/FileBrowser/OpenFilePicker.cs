using System.IO;
using FightDojo.Database;
using FightDojo.UI.Windows;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class OpenFilePicker : MonoBehaviour
  {
    private GameDataProvider gameDataProvider;
    private CharacterDataProvider characterDataProvider;
    private ComboDataProvider comboDataProvider;
    private WarningWindow warningWindow;

    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();

    private void Awake()
    {
      gameDataProvider = FindFirstObjectByType<GameDataProvider>();
      characterDataProvider = FindFirstObjectByType<CharacterDataProvider>();
      comboDataProvider = FindFirstObjectByType<ComboDataProvider>();
      warningWindow = FindFirstObjectByType<WarningWindow>();
    }

    // Вызывай эту функцию, например, по кнопке "Сохранить"
    public void OpenFile()
    {
      string lastPath = DBService.DatabasePath;
      if (Directory.Exists(lastPath) == false && File.Exists(lastPath) == false)
        lastPath = DBService.PersistentPath;

      SimpleFileBrowser.FileBrowser.SetFilters(
        true,
        new SimpleFileBrowser.FileBrowser.Filter("DB", ".db")
      );
      SimpleFileBrowser.FileBrowser.SetDefaultFilter("DB");
      SimpleFileBrowser.FileBrowser.ShowLoadDialog(
        (string[] paths) => { OnFileSelected(paths); }, // успех
        () => { Debug.Log("Отмена открытия"); }, // отмена
        SimpleFileBrowser.FileBrowser.PickMode.Files, // сохраняем файл (не папку)
        false, // только один файл
        lastPath, // начальный путь (null = Documents / стандартная папка)
        null, // предложенное имя файла по умолчанию
        "Открыть файл", // заголовок окна
        "Открыть" // текст кнопки
      );
    }

    private void OnFileSelected(string[] paths)
    {
      if (paths == null || paths.Length == 0)
      {
        Debug.Log("Сохранение отменено");
        return;
      }

      string path = paths[0]; // ← вот он — полный путь, куда сохранять
      Debug.Log($"Сохраняем в: {path}");

      if (DBService.TryOpenDatabase(path) == false)
      {
        warningWindow.OpenWarning("Не удалось открыть базу");
      }

      RefreshTables();
    }

    private void RefreshTables()
    {
      comboDataProvider.ResetSelectedCombo();
      comboDataProvider.RefreshCombos();
      characterDataProvider.ResetSelectedCharacter();
      characterDataProvider.RefreshCharacters();
      gameDataProvider.ResetSelectedGame();
      gameDataProvider.RefreshGames();
    }
  }
}