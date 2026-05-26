using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using FightDojo.Database;
using FightDojo.UI.Database.Combo;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.Database
{
  public class ComboDataProvider : MonoBehaviour
  {
    private List<Combos> combos = new List<Combos>();

    private Dictionary<int, ComboItemView> comboItemViews = new Dictionary<int, ComboItemView>();

    private PrintCombosView printCombosView;

    private int selectedCharacterId;
    private int selectedComboId;

    public bool HasSelectedCombo => selectedComboId > 0;
    public int Count => combos.Count;
    private IDatabaseService DBService => AllServices.Container.Single<IDatabaseService>();

    public void Initialize()
    {
      printCombosView = GameObject.FindAnyObjectByType<PrintCombosView>();
      printCombosView.Initialize(this);
    }

    public void AddCombo(string creatorName, string description, string tags)
    {
      if (selectedCharacterId == 0)
        return;

      Combos combo = new Combos()
      {
        CharacterId = selectedCharacterId,
        CreatorName = creatorName,
        Description = description,
        Tags = tags
      };

      DBService.AddCombo(combo);
      selectedComboId = combo.Id;
      Debug.Log(combo.Id);
      RefreshCombos();
    }

    public void DeleteCombo()
    {
      if (selectedComboId == 0)
        return;

      DBService.DeleteCombo(selectedComboId);

      ResetSelectedCombo();
      RefreshCombos();
    }

    public void UpdateCombo(string newCreatorName, string description, string tags)
    {
      if (selectedComboId == 0)
        return;

      DBService.UpdateCombo(selectedComboId, newCreatorName, description, tags);
      RefreshCombos();
    }

    public void UpdateComboJson(string comboJson)
    {
      if (selectedComboId == 0)
        return;

      DBService.UpdateComboJson(selectedComboId, comboJson);
      RefreshCombos();
    }

    public ReadOnlyCollection<Combos> GetAllCombos() =>
      combos.AsReadOnly();

    public void FindByTags(string tags)
    {
      string[] words = Regex.Split(tags, @"\s+")
        .Where(w => !string.IsNullOrWhiteSpace(w))
        .ToArray();
      Debug.Log("WORDS");
      words.ToList().ForEach(w => Debug.Log(w));
      List<Combos> result = combos.Where(combo =>
        {
          if (string.IsNullOrWhiteSpace(combo.Tags))
            return false;

          HashSet<string> tagsWords = combo.Tags
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase); // HashSet для быстрого поиска

          return words.All(w =>
            tagsWords.Any(tag =>
              tag.StartsWith(w, StringComparison.OrdinalIgnoreCase)));
        })
        .ToList();
      Debug.Log("RESULTS");
      result.ForEach(w => Debug.Log(w.Id + " " + w.Tags));

      comboItemViews = printCombosView.PrintCombos(result.AsReadOnly());
      if (comboItemViews.Count > 0)
      {
        SelectCombo(comboItemViews.First().Key);
      }
    }

    public void SelectCombo(int id)
    {
      if (id == 0 && combos.Count > 0)
      {
        id = combos[0].Id;
      }

      selectedComboId = id;
      HighlightSelectedCombo(selectedComboId);
      Debug.Log($"Selected combo id={id}");
    }

    public void CharacterSelected(int selectedCharacterId)
    {
      if (this.selectedCharacterId == selectedCharacterId)
      {
        RefreshCombos();
        return;
      }

      this.selectedCharacterId = selectedCharacterId;
      ResetSelectedCombo();
      RefreshCombos();
    }

    public void CurrentCombo(out Combos combos) =>
      combos = DBService.GetCombo(selectedComboId);

    public void ResetSelectedCombo() =>
      selectedComboId = 0;

    public void RefreshCombos()
    {
      combos = DBService.GetCombosByCharacter(selectedCharacterId);
      //_combos.ForEach(c => Debug.Log(c.Id + " " + c.CreatorName));
      comboItemViews = printCombosView.PrintCombos(GetAllCombos());
      SelectCombo(selectedComboId);
    }

    private void HighlightSelectedCombo(int id)
    {
      foreach (ComboItemView item in comboItemViews.Values)
      {
        item.Unselect();
      }

      if (id > 0 && comboItemViews.ContainsKey(id))
        comboItemViews[id].Highlight();
    }
  }
}