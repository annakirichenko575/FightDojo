using System.IO;
using FightDojo.Database;
using FightDojo.UI.Windows;
using FightDojo.Services;
using SimpleFileBrowser;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class ImportFilePicker : MonoBehaviour
  {
    private GameDataProvider gameDataProvider;
    private WarningWindow warningWindow;
    private string lastPath;

    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();
  
    private void Awake()
    {
      gameDataProvider = FindAnyObjectByType<GameDataProvider>();
      warningWindow = FindFirstObjectByType<WarningWindow>();
    }

    public void OpenFileDialog()
    {
      if (string.IsNullOrWhiteSpace(lastPath))
      {
        lastPath = DBService.DatabasePath;
        if (Directory.Exists(lastPath) == false && File.Exists(lastPath) == false)
          lastPath = DBService.PersistentPath;
      }

      FileBrowser.SetFilters( 
        true,
        new FileBrowser.Filter( "DB", ".db")
      );
      FileBrowser.SetDefaultFilter("DB");
      FileBrowser.ShowLoadDialog( 
        ( string[] paths ) => { OnFileSelected( paths ); },    // успех
        () => { Debug.Log( "Отмена" ); },                      // отмена
        FileBrowser.PickMode.Files,                            // выбираем файлы (не папки)
        false,                                                 // только один файл
        lastPath,                                                  // начальный путь (null = Documents / стандартная папка)
        null,                                                  // начальное имя файла
        "Выберите файл",                                       // заголовок окна
        "Ипортировать"                                              // текст кнопки
      );
    }

    private void OnFileSelected( string[] paths )
    {
      if( paths == null || paths.Length == 0 )
      {
        Debug.Log( "Ничего не выбрано" );
        return;
      }

      string selectedPath = paths[0];  // ← вот он — выбранный полный путь
      Debug.Log( "Выбран файл: " + selectedPath );
      lastPath = Path.GetDirectoryName(selectedPath);
      if (DBService.TryMergeDatabases(selectedPath) == false)
      {
        warningWindow.OpenWarning("Не удалось импортировать базу");
      }
      
      gameDataProvider.RefreshGames();
    }
  }
}
