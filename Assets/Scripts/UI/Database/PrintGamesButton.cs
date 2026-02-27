using UnityEngine;

public class PrintGamesButton : MonoBehaviour
{
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void PrintGames()
    {
        _gameDataProvider.PrintAllGameNames();
    }
}