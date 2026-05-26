using FightDojo.Database;
using UnityEngine;

namespace FightDojo.UI.Database.Character
{
  public class DeleteCharacterButton : MonoBehaviour
  {
    private CharacterDataProvider characterDataProvider;

    private void Awake()
    {
      characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    public void DeleteCharacter()
    {
      characterDataProvider.DeleteCharacter();
    }
  }
}