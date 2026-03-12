using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using UnityEngine;

public class PrintCharactersView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    private CharacterDataProvider characterDataProvider;

    IAssetProvider AssetProvider => AllServices.Container.Single<IAssetProvider>();

    public void Initialize(CharacterDataProvider gameDataProvider)
    {
        this.characterDataProvider = gameDataProvider;
    }

    public Dictionary<int, CharacterItemView> PrintCharacters(ReadOnlyCollection<Character> characters)
    {
        Dictionary<int, CharacterItemView> characterItemViews = new Dictionary<int, CharacterItemView>();

        foreach (Transform item in _content)
        {
            GameObject.Destroy(item.gameObject);
        }

        if (characters == null || characters.Count == 0)
            return characterItemViews;

        foreach (var character in characters)
        {
            GameObject item = AssetProvider.Instantiate(AssetPath.CharacterItemPath, _content);
            CharacterItemView itemView = item.GetComponent<CharacterItemView>();
            itemView.Initialize(character.Id, character.Name, characterDataProvider);
            characterItemViews.Add(character.Id, itemView);
        }

        return characterItemViews;
    }
}