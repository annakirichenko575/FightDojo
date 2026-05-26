using FightDojo.Database;
using UnityEngine;

namespace FightDojo.UI.Database.Game
{
  public class DeleteGameButton : MonoBehaviour
  {
    private GameDataProvider gameDataProvider;

    private void Awake()
    {
      gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void DeleteGame()
    {
      gameDataProvider.DeleteGame();
    }
  }
}