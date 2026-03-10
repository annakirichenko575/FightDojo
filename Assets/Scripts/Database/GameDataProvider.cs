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
    private int selectedGameId;
    private int selectedCharacterId;
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
        if (selectedGameId == 0)
            return;
            
        dbService.DeleteGame(selectedGameId);
        RefreshGames();
    }
    
    public ReadOnlyCollection<Game> GetAllGameNames() => 
        games.AsReadOnly();

    public void UpdateGameName(string newName)
    {
        if (selectedGameId == 0)
            return;
        
        dbService.UpdateGameName(selectedGameId, newName);
        RefreshGames();
    }

    private void RefreshGames()
    {
        games = dbService.GetAllGames();
        gameItemViews = printGamesView.PrintGames(GetAllGameNames());
        selectedGameId = 0;
        HighlightSelected(selectedGameId);
    }

    public void SelectGame(int id)
    {
        selectedGameId = id;
        HighlightSelected(selectedGameId);
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
