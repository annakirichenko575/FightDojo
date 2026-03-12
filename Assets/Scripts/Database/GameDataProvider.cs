using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Services;
using UnityEngine;

public class GameDataProvider : MonoBehaviour
{
    private List<Game> _games = new List<Game>();
    private Dictionary<int, GameItemView> _gameItemViews = new Dictionary<int, GameItemView>();

    private PrintGamesView _printGamesView;
    private CharacterDataProvider _characterDataProvider;

    private int _selectedGameId;

    private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();

    private void Start()
    {
        _printGamesView = FindAnyObjectByType<PrintGamesView>();
        _printGamesView.Initialize(this);

        _characterDataProvider = GetComponent<CharacterDataProvider>();
        _characterDataProvider.Initialize();

        FindAnyObjectByType<GameDataInput>().Initialize(this);

        RefreshGames();
    }

    public void AddGame(string name)
    {
        Game game = new Game()
        {
            Name = name,
        };

        int id = _dbService.AddGame(game);
        game.Id = id;

        RefreshGames();
        Debug.Log(id);
    }

    public void DeleteGame()
    {
        if (_selectedGameId == 0)
            return;

        _dbService.DeleteGame(_selectedGameId);
        RefreshGames();
    }

    public ReadOnlyCollection<Game> GetAllGameNames() =>
        _games.AsReadOnly();

    public void UpdateGameName(string newName)
    {
        if (_selectedGameId == 0)
            return;

        _dbService.UpdateGameName(_selectedGameId, newName);
        RefreshGames();
    }

    public void SelectGame(int id)
    {
        _selectedGameId = id;
        HighlightSelectedGame(_selectedGameId);
        _characterDataProvider.GameSelected(_selectedGameId);
    }

    private void RefreshGames()
    {
        _games = _dbService.GetAllGames();
        _gameItemViews = _printGamesView.PrintGames(GetAllGameNames());
        SelectGame(0);
    }

    private void HighlightSelectedGame(int id)
    {
        foreach (GameItemView item in _gameItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0)
            _gameItemViews[id].Highlight();
    }
}