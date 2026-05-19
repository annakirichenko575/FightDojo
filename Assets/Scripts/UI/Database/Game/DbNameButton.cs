using System;
using System.Diagnostics;
using System.IO;
using FightDojo.Database;
using Services;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Runtime.InteropServices;

namespace FightDojo.UI.Database.Game
{
    public class DbNameButton : MonoBehaviour
    {
        public void Apply()
        {
            IDatabaseService dbService = AllServices.Container.Single<IDatabaseService>();
            OpenFolder(dbService.DatabasePath);
        }
        
        private void OpenFolder(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Debug.LogError("Путь недействителен или файл не существует: " + path);
                return;
            }

            path = path.Replace("/", "\\");
            try
            {
                RevealInExplorer(path);
                //Process.Start("Explorer.exe", @"/select,""" + path + "\"");
            }
            catch (Exception e)
            {
                Debug.LogError("Не удалось открыть проводник: " + e.Message);
            } 
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, IntPtr[] apidl, uint dwFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern void SHParseDisplayName(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            IntPtr bindingContext, out IntPtr pidl, uint sfgaoIn, out uint psfgaoOut);

        private void RevealInExplorer(string path)
        {
            path = path.Replace("/", "\\");
            string folder = Path.GetDirectoryName(path);

            // Получаем PIDL папки
            SHParseDisplayName(folder, IntPtr.Zero, out IntPtr nativeFolder, 0, out _);
            // Получаем PIDL файла
            SHParseDisplayName(path, IntPtr.Zero, out IntPtr nativeFile, 0, out _);

            Debug.Log($"nativeFolder: {nativeFolder}, nativeFile: {nativeFile}");

            if (nativeFolder == IntPtr.Zero || nativeFile == IntPtr.Zero)
            {
                Process.Start(folder);
                return;
            }

            SHOpenFolderAndSelectItems(nativeFolder, 1, new[] { nativeFile }, 0);

            Marshal.FreeCoTaskMem(nativeFolder);
            Marshal.FreeCoTaskMem(nativeFile);
        }
    }
}
