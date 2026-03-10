using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Services;
using UnityEngine;

public class GameDataProvider : MonoBehaviour
{
    public List<Game> games = new List<Game>();
    public List<Character> characters = new List<Character>();

    Dictionary<int, GameItemView> gameItemViews = new Dictionary<int, GameItemView>();
    Dictionary<int, CharacterItemView> characterItemViews = new Dictionary<int, CharacterItemView>();

    private PrintGamesView printGamesView;
    private PrintCharactersView printCharactersView;

    private int selectedGameId;
    private int selectedCharacterId;

    private IDatabaseService dbService => AllServices.Container.Single<IDatabaseService>();

    private void Start()
    {
        printGamesView = FindAnyObjectByType<PrintGamesView>();
        printGamesView.Initialize(this);

        printCharactersView = FindAnyObjectByType<PrintCharactersView>();
        printCharactersView.Initialize(this);

        FindAnyObjectByType<GameDataInput>().Initialize(this);

        RefreshGames();
        RefreshCharacters();
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

    public void AddCharacter(string name)
    {
        if (selectedGameId == 0)
            return;

        Character character = new Character()
        {
            Name = name,
            GameId = selectedGameId
        };

        int id = dbService.AddCharacter(character);
        character.Id = id;

        RefreshCharacters();
        Debug.Log(id);
    }

    public void DeleteGame()
    {
        if (selectedGameId == 0)
            return;

        dbService.DeleteGame(selectedGameId);
        RefreshGames();
        RefreshCharacters();
    }

    public void DeleteCharacter()
    {
        if (selectedCharacterId == 0)
            return;

        dbService.DeleteCharacter(selectedCharacterId);
        RefreshCharacters();
    }

    public ReadOnlyCollection<Game> GetAllGameNames() =>
        games.AsReadOnly();

    public ReadOnlyCollection<Character> GetAllCharacterNames() =>
        characters.AsReadOnly();

    public void UpdateGameName(string newName)
    {
        if (selectedGameId == 0)
            return;

        dbService.UpdateGameName(selectedGameId, newName);
        RefreshGames();
    }

    public void UpdateCharacterName(string newName)
    {
        if (selectedCharacterId == 0)
            return;

        dbService.UpdateCharacterName(selectedCharacterId, newName);
        RefreshCharacters();
    }

    private void RefreshGames()
    {
        games = dbService.GetAllGames();
        gameItemViews = printGamesView.PrintGames(GetAllGameNames());
        selectedGameId = 0;
        HighlightSelectedGame(selectedGameId);
    }

    private void RefreshCharacters()
    {
        if (selectedGameId == 0)
        {
            characters = new List<Character>();
            characterItemViews = printCharactersView.PrintCharacters(GetAllCharacterNames());
            selectedCharacterId = 0;
            HighlightSelectedCharacter(selectedCharacterId);
            return;
        }

        characters = dbService.GetCharactersByGame(selectedGameId);
        characterItemViews = printCharactersView.PrintCharacters(GetAllCharacterNames());
        selectedCharacterId = 0;
        HighlightSelectedCharacter(selectedCharacterId);
    }

    public void SelectGame(int id)
    {
        selectedGameId = id;
        HighlightSelectedGame(selectedGameId);
        RefreshCharacters();
    }

    public void SelectCharacter(int id)
    {
        selectedCharacterId = id;
        HighlightSelectedCharacter(selectedCharacterId);
        Debug.Log($"Selected character id={id}");
    }

    private void HighlightSelectedGame(int id)
    {
        foreach (GameItemView item in gameItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0)
            gameItemViews[id].Highlight();
    }

    private void HighlightSelectedCharacter(int id)
    {
        foreach (CharacterItemView item in characterItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0)
            characterItemViews[id].Highlight();
    }
}