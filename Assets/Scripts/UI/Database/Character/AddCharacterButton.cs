using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Character
{
  public class AddCharacterButton : MonoBehaviour
  {
    public TMP_InputField nameInput;

    private CharacterDataProvider characterDataProvider;
    private WarningWindow warningWindow;

    private void Awake()
    {
      characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
      warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    private void OnEnable()
    {
      //nameInput.text = "";
    }

    public void AddCharacter()
    {
      string newName = nameInput.text.Trim();
      if (string.IsNullOrWhiteSpace(newName))
      {
        warningWindow.OpenWarning("Новое название не введено!");
        return;
      }

      characterDataProvider.AddCharacter(newName);
      nameInput.text = "";
    }
  }
}