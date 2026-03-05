using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using UnityEngine;

public class PrintGamesView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    private GameDataProvider gameDataProvider;

    IAssetProvider AssetProvider => AllServices.Container.Single<IAssetProvider>();
    
    public void Initialize(GameDataProvider gameDataProvider)
    {
        this.gameDataProvider = gameDataProvider;
    }
    
    public Dictionary<int, GameItemView> PrintGames(ReadOnlyCollection<Game> games)
    {
        Dictionary<int, GameItemView> gameItemViews = new Dictionary<int, GameItemView>();
        if (games == null || games.Count == 0)
        {
            //_content.text = "В базе нет игр.";
            return gameItemViews;
        }
        //_content.text = "Список игр:\n";
        foreach (Transform item in _content)
        {
            GameObject.Destroy(item.gameObject);
        }
        
        foreach (var game in games)
        {
            GameObject item = AssetProvider.Instantiate(AssetPath.GameItemPath, _content);
            GameItemView itemView = item.GetComponent<GameItemView>();
            itemView.Initialize(game.Id, game.Name, gameDataProvider);
            gameItemViews.Add(game.Id, itemView);
        }
        return gameItemViews;
    }

}