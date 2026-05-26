using System.IO;
using FightDojo.Database;
using FightDojo.UI.Windows;
using FightDojo.Services;
using SimpleFileBrowser;
using UnityEngine;

namespace FightDojo.FilePiker
{
  public class ExportFilePicker : MonoBehaviour
  {
    private WarningWindow _warningWindow;
    private string _lastPath;

    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();

    private void Awake()
    {
      _warningWindow = FindFirstObjectByType<WarningWindow>();
    }

    // Вызывай эту функцию, например, по кнопке "Сохранить"
    public void ExportFile()
    {
      if (string.IsNullOrWhiteSpace(_lastPath))
      {
        _lastPath = DBService.DatabasePath;
        if (Directory.Exists(_lastPath) == false && File.Exists(_lastPath) == false)
          _lastPath = DBService.PersistentPath;
      }

      FileBrowser.SetFilters(
        true,
        new FileBrowser.Filter("DB", ".db")
      );
      FileBrowser.SetDefaultFilter("DB");
      FileBrowser.ShowSaveDialog(
        (string[] paths) => { OnFileSelected(paths); }, // успех
        () => { Debug.Log("Отмена сохранения"); }, // отмена
        FileBrowser.PickMode.Files, // сохраняем файл (не папку)
        false, // только один файл
        _lastPath,
        "MyExport.db", // предложенное имя файла по умолчанию
        "Сохранить файл", // заголовок окна
        "Сохранить" // текст кнопки
      );
    }

    private void OnFileSelected(string[] paths)
    {
      if (paths == null || paths.Length == 0)
      {
        Debug.Log("Сохранение отменено");
        return;
      }

      string savePath = paths[0]; // ← вот он — полный путь, куда сохранять
      Debug.Log($"Сохраняем в: {savePath}");
      _lastPath = Path.GetDirectoryName(savePath);
      if (DBService.ExportDatabase(savePath) == false)
      {
        _warningWindow.OpenWarning("Не удалось экспортировать базу");
      }
    }
  }
}