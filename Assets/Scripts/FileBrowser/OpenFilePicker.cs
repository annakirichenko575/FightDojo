using System;
using FightDojo.Database;
using Services;
using SimpleFileBrowser;
using UnityEngine;

public class OpenFilePicker : MonoBehaviour
{
    
    private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();
    private GameDataProvider _gameDataProvider;
    private CharacterDataProvider _characterDataProvider;
    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindFirstObjectByType<GameDataProvider>();
        _characterDataProvider = FindFirstObjectByType<CharacterDataProvider>();
        _comboDataProvider = FindFirstObjectByType<ComboDataProvider>();
    }

    // Вызывай эту функцию, например, по кнопке "Сохранить"
    public void OpenFile()
    {
        FileBrowser.SetFilters( 
          true,
          new FileBrowser.Filter( "DB", ".db")
        );
        FileBrowser.SetDefaultFilter("DB");
        FileBrowser.ShowLoadDialog( 
            ( string[] paths ) => { OnFileSelected( paths ); },   // успех
            () => { Debug.Log( "Отмена открытия" ); },              // отмена
            FileBrowser.PickMode.Files,                               // сохраняем файл (не папку)
            false,                                                    // только один файл
            _dbService.PersistentPath,                                                  // начальный путь (null = Documents / стандартная папка)
            null,                                           // предложенное имя файла по умолчанию
            "Открыть файл",                                         // заголовок окна
            "Открыть"                                               // текст кнопки
        );
    }

    private void OnFileSelected( string[] paths )
    {
        if( paths == null || paths.Length == 0 )
        {
            Debug.Log( "Сохранение отменено" );
            return;
        }

        string path = paths[0];  // ← вот он — полный путь, куда сохранять
        Debug.Log( $"Сохраняем в: {path}" );
        _dbService.OpenDatabase(path);
        RefreshTables();
    }

    private void RefreshTables()
    {
        _comboDataProvider.ResetSelectedCombo();
        _comboDataProvider.RefreshCombos();
        _characterDataProvider.ResetSelectedCharacter();
        _characterDataProvider.RefreshCharacters();
        _gameDataProvider.ResetSelectedGame();
        _gameDataProvider.RefreshGames();
    }
}
