using UnityEngine;

public class DeleteCharacterButton : MonoBehaviour
{
    private CharacterDataProvider _characterDataProvider;

    private void Awake()
    {
        _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    public void DeleteCharacter()
    {
        _characterDataProvider.DeleteCharacter();
    }
}