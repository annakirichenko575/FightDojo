using UnityEngine;

public class DeleteGameButton : MonoBehaviour
{
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void DeleteGame()
    {
        _gameDataProvider.DeleteGame();
    }
}
