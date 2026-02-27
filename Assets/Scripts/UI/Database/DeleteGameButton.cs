using System;
using TMPro;
using UnityEngine;

public class DeleteGameButton : MonoBehaviour
{
    public TMP_InputField nameTMP;
    
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void DeleteGame()
    {
        _gameDataProvider.DeleteGame(Convert.ToInt32(nameTMP.text));
    }
}
