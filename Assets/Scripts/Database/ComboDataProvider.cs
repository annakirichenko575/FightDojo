using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        int id = _dbService.AddCombo(combo);
        combo.Id = id;

        RefreshCombos();
        Debug.Log(id);
    }

    public void DeleteCombo()
    {
        if (_selectedComboId == 0)
            return;

        _dbService.DeleteCombo(_selectedComboId);
        RefreshCombos();
    }

    public ReadOnlyCollection<Combos> GetAllCombos() =>
        _combos.AsReadOnly();

    public void UpdateCombo(string newCreatorName)
    {
        if (_selectedComboId == 0)
            return;

        _dbService.UpdateCombo(_selectedComboId, newCreatorName);
        RefreshCombos();
    }

    public void SelectCombo(int id)
    {
        _selectedComboId = id;
        HighlightSelectedCombo(_selectedComboId);
        Debug.Log($"Selected combo id={id}");
    }

    public void CharacterSelected(int selectedCharacterId)
    {
        _selectedCharacterId = selectedCharacterId;
        RefreshCombos();
    }

    private void RefreshCombos()
    {
        _combos = _dbService.GetCombosByCharacter(_selectedCharacterId);
        _comboItemViews = _printCombosView.PrintCombos(GetAllCombos());
        SelectCombo(0);
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