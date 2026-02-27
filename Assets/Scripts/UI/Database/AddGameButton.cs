using TMPro;
using UnityEngine;

public class AddGameButton : MonoBehaviour
{
    public TMP_InputField nameTMP;
    
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void AddGame()
    {
        _gameDataProvider.AddGame(nameTMP.text);
    }
}
