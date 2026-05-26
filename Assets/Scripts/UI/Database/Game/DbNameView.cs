using FightDojo.Database;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
  [RequireComponent(typeof(TMP_Text))]
  public class DbNameView : MonoBehaviour
  {
    private TMP_Text dbName;
    private IDatabaseService dbService;

    public void Initialize(IDatabaseService dbService)
    {
      this.dbService = dbService;
      this.dbName = GetComponent<TMP_Text>();
    }
    
    public void PrintDbPath()
    {
      dbName.text = dbService.DatabasePath;
    }
  }
}