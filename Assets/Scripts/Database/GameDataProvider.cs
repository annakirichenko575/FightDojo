using System;
using System.Collections.Generic;
using FightDojo.Database;
using Services;
using UnityEngine;

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
        RefreshGames();
    }

    private void RefreshGames()
    {
        games = dbService.GetAllGames();
    }

    public void DeleteGame(int id)
    {
        dbService.DeleteGame(id);
    }
}
