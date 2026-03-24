using System;
using FightDojo.Database;
using Services;
using SimpleFileBrowser;
using UnityEngine;

public class ImportFilePicker : MonoBehaviour
{
  private GameDataProvider _gameDataProvider;
  
  private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();
  
  private void Awake()
  {
      _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
  }

  public void OpenFileDialog()
  {
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
      _dbService.PersistentPath,                                                  // начальный путь (null = Documents / стандартная папка)
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
    _dbService.MergeDatabases(selectedPath);
    _gameDataProvider.RefreshGames();
  }
}
