using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using FightDojo.Database;
using Services;
using UnityEngine;

public class ComboDataProvider : MonoBehaviour
{
    private List<Combos> _combos = new List<Combos>();

    private Dictionary<int, ComboItemView> _comboItemViews = new Dictionary<int, ComboItemView>();

    private PrintCombosView _printCombosView;

    private int _selectedCharacterId;
    private int _selectedComboId;

    private IDatabaseService _dbService => AllServices.Container.Single<IDatabaseService>();

    public void Initialize()
    {
        _printCombosView = GameObject.FindAnyObjectByType<PrintCombosView>();
        _printCombosView.Initialize(this);
    }

    public void AddCombo(string creatorName, string description, string tags)
    {
        if (_selectedCharacterId == 0)
            return;

        Combos combo = new Combos()
        {
            CharacterId = _selectedCharacterId,
            CreatorName = creatorName,
            Description = description,
            Tags = tags
        };

        _dbService.AddCombo(combo);
        _selectedComboId = combo.Id;
        Debug.Log(combo.Id);
        RefreshCombos();
    }

    public void DeleteCombo()
    {
        if (_selectedComboId == 0)
            return;

        _dbService.DeleteCombo(_selectedComboId);
        
        ResetSelectedCombo();
        RefreshCombos();
    }
    
    public void UpdateCombo(string newCreatorName, string description, string tags)
    {
        if (_selectedComboId == 0)
            return;

        _dbService.UpdateCombo(_selectedComboId, newCreatorName, description, tags);
        RefreshCombos();
    }

    public void UpdateComboJson(string comboJson)
    {
        if (_selectedComboId == 0)
            return;

        _dbService.UpdateComboJson(_selectedComboId, comboJson);
        RefreshCombos();
    }
    
    public ReadOnlyCollection<Combos> GetAllCombos() =>
        _combos.AsReadOnly();

    public void FindByTags(string tags)
    {
        string[] words = Regex.Split(tags, @"\s+")
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToArray(); 
        Debug.Log("WORDS");
        words.ToList().ForEach(w => Debug.Log(w));
        List<Combos> result = _combos.Where(combo =>
            {
                if (string.IsNullOrWhiteSpace(combo.Tags))
                    return false;

                HashSet<string> tagsWords = combo.Tags
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);   // HashSet для быстрого поиска

                return words.All(w => 
                    tagsWords.Any(tag =>
                        tag.StartsWith(w, StringComparison.OrdinalIgnoreCase)));
            })
            .ToList();
        Debug.Log("RESULTS");
        result.ForEach(w => Debug.Log(w.Id + " " + w.Tags));

        _comboItemViews = _printCombosView.PrintCombos(result.AsReadOnly());
        if (_comboItemViews.Count > 0)
        {
            SelectCombo(_comboItemViews.First().Key);
        } 
    }

    public void SelectCombo(int id)
    {
        if (id == 0 && _combos.Count > 0)
        {
            id = _combos[0].Id;
        }
        _selectedComboId = id;
        HighlightSelectedCombo(_selectedComboId);
        Debug.Log($"Selected combo id={id}");
    }

    public void CharacterSelected(int selectedCharacterId)
    {
        if (_selectedCharacterId == selectedCharacterId)
        {
            RefreshCombos();
            return;
        }
        
        _selectedCharacterId = selectedCharacterId;
        ResetSelectedCombo();
        RefreshCombos();
    }

    public void CurrentCombo(out Combos combos) => 
        combos = _dbService.GetCombo(_selectedComboId);

    public void ResetSelectedCombo() => 
        _selectedComboId = 0;

    public void RefreshCombos()
    {
        _combos = _dbService.GetCombosByCharacter(_selectedCharacterId);
        _comboItemViews = _printCombosView.PrintCombos(GetAllCombos());
        SelectCombo(_selectedComboId);
    }

    private void HighlightSelectedCombo(int id)
    {
        foreach (ComboItemView item in _comboItemViews.Values)
        {
            item.Unselect();
        }

        if (id > 0 && _comboItemViews.ContainsKey(id))
            _comboItemViews[id].Highlight();
    }

}