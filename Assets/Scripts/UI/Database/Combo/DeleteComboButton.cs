using FightDojo.Database;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
  public class DeleteComboButton : MonoBehaviour
  {
    private ComboDataProvider comboDataProvider;

    private void Awake()
    {
      comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
    }

    public void DeleteCombo()
    {
      comboDataProvider.DeleteCombo();
    }
  }
}