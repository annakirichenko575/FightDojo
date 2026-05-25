using System.IO;
using FightDojo.Database;
using FightDojo.UI.Windows;
using Services;
using SimpleFileBrowser;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class ImportFilePicker : MonoBehaviour
  {
    private GameDataProvider _gameDataProvider;
    private WarningWindow _warningWindow;
    private string _lastPath;

    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();
  
    private void Awake()
    {
      _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
      _warningWindow = FindFirstObjectByType<WarningWindow>();
    }

    public void OpenFileDialog()
    {
      if (string.IsNullOrWhiteSpace(_lastPath))
      {
        _lastPath = DBService.DatabasePath;
        if (Directory.Exists(_lastPath) == false && File.Exists(_lastPath) == false)
          _lastPath = DBService.PersistentPath;
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
        _lastPath,                                                  // начальный путь (null = Documents / стандартная папка)
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
      _lastPath = Path.GetDirectoryName(selectedPath);
      if (DBService.TryMergeDatabases(selectedPath) == false)
      {
        _warningWindow.OpenWarning("Не удалось импортировать базу");
      }
      
      _gameDataProvider.RefreshGames();
    }
  }
}
