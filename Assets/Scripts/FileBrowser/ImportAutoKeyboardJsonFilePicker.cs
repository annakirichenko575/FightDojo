using System;
using FightDojo;
using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using FightDojo.Database;
using Services;
using SimpleFileBrowser;
using UnityEngine;

public class ImportAutoKeyboardJsonFilePicker : MonoBehaviour
{
  private EditorComboStrip _editorComboStrip;
  
  private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();
  
  private void Awake()
  {
      _editorComboStrip = FindAnyObjectByType<EditorComboStrip>();
  }

  public void OpenFileDialog()
  {
    FileBrowser.SetFilters( 
      true,
      new FileBrowser.Filter( "JSON", ".json")
    );
    FileBrowser.SetDefaultFilter("JSON");
    FileBrowser.ShowLoadDialog( 
      ( string[] paths ) => { OnFileSelected( paths ); },    // успех
      () => { Debug.Log( "Отмена" ); },                      // отмена
      FileBrowser.PickMode.Files,                            // выбираем файлы (не папки)
      false,                                                 // только один файл
      GameDirectory.GetPath(),                                                  // начальный путь (null = Documents / стандартная папка)
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
    
    JsonLoader jsonLoader = new JsonLoader();
    RecordedKeys tempRecordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load(selectedPath));
    string adaptedJson = tempRecordedKeys.ToJson();
    IRecordedKeysService recordedKeys = 
      AllServices.Container.Single<IRecordedKeysService>();
    recordedKeys.LoadJson(adaptedJson);
    _editorComboStrip.Open();
  }
}
