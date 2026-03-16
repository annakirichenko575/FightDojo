using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using UnityEngine;

public class PrintCombosView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    private ComboDataProvider comboDataProvider;

    IAssetProvider AssetProvider => AllServices.Container.Single<IAssetProvider>();

    public void Initialize(ComboDataProvider comboDataProvider)
    {
        this.comboDataProvider = comboDataProvider;
    }

    public Dictionary<int, ComboItemView> PrintCombos(ReadOnlyCollection<Combos> combos)
    {
        Dictionary<int, ComboItemView> comboItemViews = new Dictionary<int, ComboItemView>();

        foreach (Transform item in _content)
        {
            GameObject.Destroy(item.gameObject);
        }

        if (combos == null || combos.Count == 0)
            return comboItemViews;

        foreach (var combo in combos)
        {
            GameObject item = AssetProvider.Instantiate(AssetPath.ComboItemPath, _content);
            ComboItemView itemView = item.GetComponent<ComboItemView>();
            itemView.Initialize(combo.Id, combo.CreatorName, combo.Description, combo.Tags, comboDataProvider);
            comboItemViews.Add(combo.Id, itemView);
        }

        return comboItemViews;
    }
}