using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Services;
using UnityEngine;

public class CharacterDataProvider : MonoBehaviour
{
    private List<Character> _characters = new List<Character>();

    private Dictionary<int, CharacterItemView> _characterItemViews = new Dictionary<int, CharacterItemView>();

    private PrintCharactersView _printCharactersView;

    private ComboDataProvider _comboDataProvider;

    private int _selectedGameId;
    private int _selectedCharacterId;

    private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();

    public void Initialize()
    {
        _printCharactersView = GameObject.FindAnyObjectByType<PrintCharactersView>();
        _printCharactersView.Initialize(this);

        // находим ComboDataProvider
        _comboDataProvider = GetComponent<ComboDataProvider>();
        _comboDataProvider.Initialize();
    }

    public void AddCharacter(string name)
    {
        if (_selectedGameId == 0)
            return;

        Character character = new Character()
        {
            Name = name,
            GameId = _selectedGameId
        };

        _dbService.AddCharacter(character);
        _selectedCharacterId = character.Id;
        Debug.Log(character.Id);
        RefreshCharacters();
    }

    public void DeleteCharacter()
    {
        if (_selectedCharacterId == 0)
            return;

        _dbService.DeleteCharacter(_selectedCharacterId);
        
        ResetSelectedCharacter();
        RefreshCharacters();
    }

    public ReadOnlyCollection<Character> GetAllCharacterNames() =>
        _characters.AsReadOnly();

    public void UpdateCharacterName(string newName)
    {
        if (_selectedCharacterId == 0)
            return;

        _dbService.UpdateCharacterName(_selectedCharacterId, newName);
        RefreshCharacters();
    }

    public void SelectCharacter(int id)
    {
        if (id == 0 && _characters.Count > 0)
        {
            id = _characters[0].Id;
        }
        _selectedCharacterId = id;
        HighlightSelectedCharacter(_selectedCharacterId);

        Debug.Log($"Selected character id={id}");

        // передаем выбранного персонажа в ComboDataProvider
        _comboDataProvider.CharacterSelected(_selectedCharacterId);
    }

    public void GameSelected(int selectedGameId)
    {
        if (_selectedGameId == selectedGameId)
        {
            RefreshCharacters();
            return;
        }
        
        _selectedGameId = selectedGameId;
        ResetSelectedCharacter();
        RefreshCharacters();
    }

    public void CurrentCharacter(out Character character) => 
        character = _dbService.GetCharacter(_selectedCharacterId);

    public void ResetSelectedCharacter() => 
        _selectedCharacterId = 0;

    public void RefreshCharacters()
    { 
        _characters = _dbService.GetCharactersByGame(_selectedGameId);
        _characterItemViews = _printCharactersView.PrintCharacters(GetAllCharacterNames());
        SelectCharacter(_selectedCharacterId);
    }

    private void HighlightSelectedCharacter(int id)
    {
        foreach (CharacterItemView item in _characterItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0)
            _characterItemViews[id].Highlight();
    }

}