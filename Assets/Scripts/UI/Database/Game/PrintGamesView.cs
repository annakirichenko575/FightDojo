using System.Collections.Generic;
using System.Collections.ObjectModel;
using FightDojo.Database;
using FightDojo.Infrastructure.AssetManagement;
using FightDojo.Services;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
    public class PrintGamesView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        private GameDataProvider gameDataProvider;

        IAssetProvider AssetProvider => AllServices.Container.Single<IAssetProvider>();
    
        public void Initialize(GameDataProvider gameDataProvider)
        {
            this.gameDataProvider = gameDataProvider;
        }
    
        public Dictionary<int, GameItemView> PrintGames(ReadOnlyCollection<FightDojo.Database.Game> games)
        {
            Dictionary<int, GameItemView> gameItemViews = new Dictionary<int, GameItemView>();
            //_content.text = "Список игр:\n";
            foreach (Transform item in _content)
            {
                GameObject.Destroy(item.gameObject);
            }
        
            if (games == null || games.Count == 0)
            {
                //_content.text = "В базе нет игр.";
                return gameItemViews;
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
}