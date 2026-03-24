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

        _dbService.AddGame(game);
        _selectedGameId = game.Id;
        Debug.Log(game.Id);
        RefreshGames();
    }

    public void DeleteGame()
    {
        if (_selectedGameId == 0)
            return;

        _dbService.DeleteGame(_selectedGameId);
        ResetSelectedGame();
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
        if (id == 0 && _games.Count > 0)
        {
            id = _games[0].Id;
        }
        _selectedGameId = id;
        HighlightSelectedGame(_selectedGameId);
        _characterDataProvider.GameSelected(_selectedGameId);
    }

    public void CurrentGame(out Game game) => 
        game = _dbService.GetGame(_selectedGameId);

    public void RefreshGames()
    {
        _games = _dbService.GetAllGames();
        _gameItemViews = _printGamesView.PrintGames(GetAllGameNames());
        SelectGame(_selectedGameId);
    }

    public void ResetSelectedGame() => 
        _selectedGameId = 0;

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