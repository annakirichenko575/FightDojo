using System;
using UnityEngine;

public class GameDataInput : MonoBehaviour
{
    private GameDataProvider gameDataProvider;

    public void Initialize(GameDataProvider gameDataProvider)
    {
        this.gameDataProvider = gameDataProvider;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            gameDataProvider.DeleteGame();
        }
    }
}
