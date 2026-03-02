using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Services;
using UnityEngine;
using TMPro;

public class GameDataProvider : MonoBehaviour
{
    public List<Game> games = new List<Game>();
    private IDatabaseService dbService => AllServices.Container.Single<IDatabaseService>();
    
    private void Start()
    {
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
        Debug.Log(id);
    }

    public void DeleteGame(int id)
    {
        dbService.DeleteGame(id);
    }
    
    public ReadOnlyCollection<Game> GetAllGameNames()
    {
        RefreshGames();
        return games.AsReadOnly();
    }
    
    public void UpdateGameName(int id, string newName)
    {
        dbService.UpdateGameName(id, newName);
    }

    private void RefreshGames()
    {
        games = dbService.GetAllGames();
    }
}
