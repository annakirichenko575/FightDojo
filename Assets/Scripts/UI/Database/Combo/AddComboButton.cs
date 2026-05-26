using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Combo
{
  public class AddComboButton : MonoBehaviour
  {
    public TMP_InputField nameInput;
    public TMP_InputField descriptionInput;
    public TMP_InputField tagsInput;

    private ComboDataProvider comboDataProvider;
    private WarningWindow warningWindow;

    private void Awake()
    {
      comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
      warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    public void AddCombo()
    {
      string newName = nameInput.text.Trim();
      if (string.IsNullOrWhiteSpace(newName))
      {
        warningWindow.OpenWarning("Новое имя автора не введено!");
        return;
      }

      comboDataProvider.AddCombo(newName, descriptionInput.text.Trim(), tagsInput.text.Trim());
      nameInput.text = "";
      descriptionInput.text = "";
      tagsInput.text = "";
    }
  }
}