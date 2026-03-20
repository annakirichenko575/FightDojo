using System;
using FightDojo.Database;
using Services;
using SimpleFileBrowser;
using UnityEngine;

public class ExportFilePicker : MonoBehaviour
{
    
    private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();

    // Вызывай эту функцию, например, по кнопке "Сохранить"
    public void ExportFile()
    {
        FileBrowser.SetFilters( 
          true,
          new FileBrowser.Filter( "DB", ".db")
        );
        FileBrowser.SetDefaultFilter("DB");
        FileBrowser.ShowSaveDialog( 
            ( string[] paths ) => { OnFileSaveSelected( paths ); },   // успех
            () => { Debug.Log( "Отмена сохранения" ); },              // отмена
            FileBrowser.PickMode.Files,                               // сохраняем файл (не папку)
            false,                                                    // только один файл
            _dbService.PersistentDbPath,                                                  // начальный путь (null = Documents / стандартная папка)
            "MyExport.db",                                           // предложенное имя файла по умолчанию
            "Сохранить файл",                                         // заголовок окна
            "Сохранить"                                               // текст кнопки
        );
    }

    private void OnFileSaveSelected( string[] paths )
    {
        if( paths == null || paths.Length == 0 )
        {
            Debug.Log( "Сохранение отменено" );
            return;
        }

        string savePath = paths[0];  // ← вот он — полный путь, куда сохранять
        Debug.Log( $"Сохраняем в: {savePath}" );
        _dbService.ExportDatabase(savePath);
    }
}
