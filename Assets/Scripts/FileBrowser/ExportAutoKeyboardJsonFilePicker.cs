using System.Collections.Generic;
using System.IO;
using System.Linq;
using FightDojo.ComboEditor;
using FightDojo.ComboEditor.Data;
using FightDojo.ComboEditor.Data.AutoKeyboard;
using FightDojo.UI.Windows;
using FightDojo.Services;
using SimpleFileBrowser;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class ExportAutoKeyboardJsonFilePicker : MonoBehaviour
  {
    private WarningWindow warningWindow;
    private string lastPath;

    private IRecordedKeysService RecordedKeys =>
      AllServices.Container.Single<IRecordedKeysService>();

    private void Awake()
    {
      warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    public void OpenFileDialog()
    {
      if (string.IsNullOrWhiteSpace(lastPath)
          || Directory.Exists(lastPath) == false && File.Exists(lastPath) == false)
      {
        lastPath = GameDirectory.GetPath();
      }

      FileBrowser.SetFilters(
        true,
        new FileBrowser.Filter("JSON", ".json")
      );
      FileBrowser.SetDefaultFilter("JSON");
      FileBrowser.ShowSaveDialog(
        (string[] paths) => { OnFileSelected(paths); }, // успех
        () => { Debug.Log("Отмена"); }, // отмена
        FileBrowser.PickMode.Files, // выбираем файлы (не папки)
        false, // только один файл
        lastPath, // начальный путь (null = Documents / стандартная папка)
        "MyAutoKeyboardCombo.json", // начальное имя файла
        "Сохранить json файл для AutoKeyboard", // заголовок окна
        "Сохранить" // текст кнопки
      );
    }

    private void OnFileSelected(string[] paths)
    {
      if (paths == null || paths.Length == 0)
      {
        Debug.Log("Экспорт отменён");
        return;
      }

      string savedPath = paths[0]; // ← вот он — выбранный полный путь
      Debug.Log("Сохранён в: " + savedPath);
      lastPath = Path.GetDirectoryName(savedPath);

      List<KeyData> keys = RecordedKeys.GetKeys().ToList();
      RecordData tempRecordData = RecordDataAdapter.Adapt(keys);
      JsonLoader jsonLoader = new JsonLoader();
      if (tempRecordData == null
          || jsonLoader.TrySaveToJsonFile(savedPath, tempRecordData) == false)
      {
        warningWindow.OpenWarning("Ошибка экспорта json");
      }
    }
  }
}