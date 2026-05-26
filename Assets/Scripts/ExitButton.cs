using FightDojo.Database;
using FightDojo.Services;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FightDojo.ComboEditor
{
    public class ExitButton : MonoBehaviour
    {
        private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();
        
        public void ExitGame()
        {
            _dbService.Dispose();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}