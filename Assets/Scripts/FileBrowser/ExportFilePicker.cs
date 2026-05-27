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
    private WarningWindow warningWindow;
    private string lastPath;

    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();

    private void Awake()
    {
      warningWindow = FindFirstObjectByType<WarningWindow>();
    }

    // Вызывай эту функцию, например, по кнопке "Сохранить"
    public void ExportFile()
    {
      if (string.IsNullOrWhiteSpace(lastPath))
      {
        lastPath = DBService.DatabasePath;
        if (Directory.Exists(lastPath) == false && File.Exists(lastPath) == false)
          lastPath = DBService.PersistentPath;
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
        lastPath,
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
      lastPath = Path.GetDirectoryName(savePath);
      if (DBService.ExportDatabase(savePath) == false)
      {
        warningWindow.OpenWarning("Не удалось экспортировать базу");
      }
    }
  }
}