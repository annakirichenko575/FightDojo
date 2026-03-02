using System.Collections.ObjectModel;
using FightDojo.Database;
using Infrastructure.AssetManagement;
using Services;
using UnityEngine;

public class PrintGamesView : MonoBehaviour
{
    [SerializeField] private Transform _content;

    private GameDataProvider _gameDataProvider;
    
    IAssetProvider AssetProvider => AllServices.Container.Single<IAssetProvider>();
    
    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void PrintGames()
    {
        ReadOnlyCollection<Game> games = _gameDataProvider.GetAllGameNames();
        if (games == null || games.Count == 0)
        {
            //_content.text = "В базе нет игр.";
            return;
        }

        //_content.text = "Список игр:\n";
        foreach (Transform item in _content)
        {
            GameObject.Destroy(item.gameObject);
        }
        
        foreach (var game in games)
        {
            GameObject item = AssetProvider.Instantiate(AssetPath.GameItemPath, _content);
            item.GetComponent<GameItemView>().Initialize(game.Id, game.Name);
        }
    }
}