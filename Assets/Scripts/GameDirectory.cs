using System.IO;
using UnityEngine;

namespace FightDojo
{
  public class GameDirectory
  {
    public static string GetPath()
    {
      string path;

      // В редакторе возвращаем папку проекта (для удобства тестирования)
      if (Application.isEditor)
      {
        path = Application.dataPath;                    // .../Assets
        path = Path.GetFullPath(Path.Combine(path, "..")); // поднимаемся на уровень выше — в корень проекта
      }
      else
      {
        // В билде
        path = Application.dataPath;

        // Windows / Linux: убираем "_Data"
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
          path = path.Replace("_Data", "");
        }
        // macOS: Application.dataPath уже ведёт внутрь .app/Contents, поднимаемся выше
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
          path = Path.GetFullPath(Path.Combine(path, "../.."));
        }
      }

      return path;
    } 
  }
}