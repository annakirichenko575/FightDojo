using System.IO;
using FightDojo.ComboEditor;
using FightDojo.ComboEditor.Data;
using FightDojo.ComboEditor.Data.AutoKeyboard;
using FightDojo.UI.Windows;
using FightDojo.Services;
using SimpleFileBrowser;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class ImportAutoKeyboardJsonFilePicker : MonoBehaviour
  {
    private EditorComboStrip _editorComboStrip;
    private WarningWindow _warningWindow;
    private string _lastPath;
  
    private void Awake()
    {
      _editorComboStrip = FindAnyObjectByType<EditorComboStrip>();
      _warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    public void OpenFileDialog()
    {
      if (string.IsNullOrWhiteSpace(_lastPath)
        || Directory.Exists(_lastPath) == false && File.Exists(_lastPath) == false)
      {
          _lastPath = GameDirectory.GetPath();
      }
      
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
        _lastPath,                                                  // начальный путь (null = Documents / стандартная папка)
        null,                                                  // начальное имя файла
        "Выберите json файл из AutoKeyboard",                                       // заголовок окна
        "Ипортировать из AutoKeyboard"                                              // текст кнопки
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
    
      JsonLoader jsonLoader = new JsonLoader();
      if (jsonLoader.TryLoad(selectedPath, out RecordData recordData)
          && recordData != null && recordData.Validate())
      {
        RecordedKeys tempRecordedKeys = RecordDataAdapter.Adapt(recordData);
        string adaptedJson = tempRecordedKeys.ToJson();
        IRecordedKeysService recordedKeys = 
          AllServices.Container.Single<IRecordedKeysService>();
        recordedKeys.LoadJson(adaptedJson);
        _editorComboStrip.Open();
        _editorComboStrip.SetChangeFlag();
      }
      else
      {
        _warningWindow.OpenWarning("Ошибка импорта json");
      }
    }
  }
}
