using FightDojo.Database;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
  [RequireComponent(typeof(TMP_Text))]
  public class DbNameView : MonoBehaviour
  {
    private TMP_Text _name;
    private IDatabaseService dbService;

    public void Initialize(IDatabaseService dbService)
    {
      this.dbService = dbService;
      this._name = GetComponent<TMP_Text>();
    }
    
    public void PrintDbPath()
    {
      _name.text = dbService.DatabasePath;
    }
  }
}