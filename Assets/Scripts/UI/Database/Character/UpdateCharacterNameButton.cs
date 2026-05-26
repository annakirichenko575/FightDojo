using FightDojo.Database;
using FightDojo.UI.Windows;
using TMPro;
using UnityEngine;

namespace FightDojo.UI.Database.Character
{
  public class UpdateCharacterNameButton : MonoBehaviour
  {
    [SerializeField] private TMP_InputField nameInput;

    private CharacterDataProvider characterDataProvider;
    private WarningWindow warningWindow;

    private void Awake()
    {
      characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
      warningWindow = FindAnyObjectByType<WarningWindow>();
    }

    private void OnEnable()
    {
      characterDataProvider.CurrentCharacter(out FightDojo.Database.Character characterData);
      nameInput.text = characterData.Name;
    }

    public void UpdateName()
    {
      string newName = nameInput.text.Trim();
      if (string.IsNullOrWhiteSpace(newName))
      {
        warningWindow.OpenWarning("Новое название не введено!");
        return;
      }

      characterDataProvider.UpdateCharacterName(newName);
      nameInput.text = "";
    }
  }
}