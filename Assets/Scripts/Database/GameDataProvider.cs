using System;
using System.Collections.Generic;
using FightDojo.Database;
using Services;
using UnityEngine;
using TMPro;

public class GameDataProvider : MonoBehaviour
{
    public List<Game> games = new List<Game>();
    private IDatabaseService dbService => AllServices.Container.Single<IDatabaseService>();
    
    [SerializeField] private TMP_Text outputText;

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
    
    public void PrintAllGameNames()
    {
        RefreshGames();

        if (games == null || games.Count == 0)
        {
            outputText.text = "В базе нет игр.";
            return;
        }

        outputText.text = "Список игр:\n";

        foreach (var game in games)
        {
            outputText.text += $"{game.Name}\n";
        }
    }
    
    public void UpdateGameName(int id, string newName)
    {
        dbService.UpdateGameName(id, newName);
        RefreshGames(); // чтобы список в памяти обновился
    }
}
