using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.UI.Database.Game;
using FightDojo.Database;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.Database
{
  public class GameDataProvider : MonoBehaviour
  {
    private List<Game> games = new List<Game>();
    private Dictionary<int, GameItemView> gameItemViews = new Dictionary<int, GameItemView>();

    private PrintGamesView printGamesView;
    private CharacterDataProvider characterDataProvider;
    private DbNameView dbNameView;
    private int selectedGameId;

    public bool HasSelectedGame => selectedGameId > 0;
    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();

    private void Start()
    {
      dbNameView = FindAnyObjectByType<DbNameView>();
      dbNameView.Initialize(DBService);
      dbNameView.PrintDbPath();

      printGamesView = FindAnyObjectByType<PrintGamesView>();
      printGamesView.Initialize(this);

      characterDataProvider = GetComponent<CharacterDataProvider>();
      characterDataProvider.Initialize();

      FindAnyObjectByType<GameDataInput>().Initialize(this);

      RefreshGames();
    }

    public void AddGame(string name)
    {
      Game game = new Game()
      {
        Name = name,
      };

      DBService.AddGame(game);
      selectedGameId = game.Id;
      Debug.Log(game.Id);
      RefreshGames();
    }

    public void DeleteGame()
    {
      if (selectedGameId == 0)
        return;

      DBService.DeleteGame(selectedGameId);
      ResetSelectedGame();
      RefreshGames();
    }

    public ReadOnlyCollection<Game> GetAllGameNames() =>
      games.AsReadOnly();

    public void UpdateGameName(string newName)
    {
      if (selectedGameId == 0)
        return;

      DBService.UpdateGameName(selectedGameId, newName);
      RefreshGames();
    }

    public void SelectGame(int id)
    {
      if (id == 0 && games.Count > 0)
      {
        id = games[0].Id;
      }

      selectedGameId = id;
      HighlightSelectedGame(selectedGameId);
      characterDataProvider.GameSelected(selectedGameId);
    }

    public void CurrentGame(out Game game) =>
      game = DBService.GetGame(selectedGameId);

    public void RefreshGames()
    {
      dbNameView.PrintDbPath();
      games = DBService.GetAllGames();
      gameItemViews = printGamesView.PrintGames(GetAllGameNames());
      SelectGame(selectedGameId);
    }

    public void ResetSelectedGame() =>
      selectedGameId = 0;

    private void HighlightSelectedGame(int id)
    {
      foreach (GameItemView item in gameItemViews.Values)
      {
        item.Unselect();
      }

      if (id > 0)
        gameItemViews[id].Highlight();
    }
  }
}