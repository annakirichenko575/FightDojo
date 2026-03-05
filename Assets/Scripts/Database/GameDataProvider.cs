using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Services;
using UnityEngine;

public class GameDataProvider : MonoBehaviour
{
    public List<Game> games = new List<Game>();
    Dictionary<int, GameItemView> gameItemViews = new Dictionary<int, GameItemView>();
    
    private PrintGamesView printGamesView;
    private int selectedId;
    private IDatabaseService dbService => AllServices.Container.Single<IDatabaseService>();
    
    private void Start()
    {
        printGamesView = FindAnyObjectByType<PrintGamesView>();
        printGamesView.Initialize(this);
        FindAnyObjectByType<GameDataInput>().Initialize(this);
        RefreshGames();
    }

    public void AddGame(string name)
    {
        Game game = new Game()
        {
            Name = name,
        };
        int id = dbService.AddGame(game);
        game.Id = id;
        
        RefreshGames();
        Debug.Log(id);
    }

    public void DeleteGame()
    {
        if (selectedId == 0)
            return;
            
        dbService.DeleteGame(selectedId);
        RefreshGames();
    }
    
    public ReadOnlyCollection<Game> GetAllGameNames() => 
        games.AsReadOnly();

    public void UpdateGameName(int id, string newName)
    {
        dbService.UpdateGameName(id, newName);
        RefreshGames();
    }

    private void RefreshGames()
    {
        games = dbService.GetAllGames();
        gameItemViews = printGamesView.PrintGames(GetAllGameNames());
        selectedId = 0;
        HighlightSelected(selectedId);
    }

    public void SelectGame(int id)
    {
        selectedId = id;
        HighlightSelected(selectedId);
    }

    private void HighlightSelected(int id)
    {
        foreach (GameItemView item in gameItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0)
            gameItemViews[id].Highlight();
    }
}
