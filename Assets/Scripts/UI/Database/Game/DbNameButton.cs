using System.Diagnostics;
using System.IO;
using FightDojo.Database;
using Services;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FightDojo.UI.Database.Game
{
    public class DbNameButton : MonoBehaviour
    {
        public void Apply()
        {
            IDatabaseService dbService = AllServices.Container.Single<IDatabaseService>();
            OpenFolder(dbService.DatabasePath);
        }
        
        private static void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Debug.LogError("Путь недействителен или файл не существует: " + path);
                return;
            }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            //Process.Start(path);
            Process.Start("Explorer.exe", @"/select,""" + path + "\"");
#endif
        }
    }
}
