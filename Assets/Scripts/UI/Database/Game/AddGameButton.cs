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
        string newName = nameTMP.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _gameDataProvider.AddGame(newName);
        nameTMP.text = "";
    }
}
