using UnityEngine;

public class DeleteCharacterButton : MonoBehaviour
{
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void DeleteCharacter()
    {
        _gameDataProvider.DeleteCharacter();
    }
}