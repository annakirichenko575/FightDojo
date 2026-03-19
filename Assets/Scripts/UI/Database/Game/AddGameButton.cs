using TMPro;
using UnityEngine;

public class AddGameButton : MonoBehaviour
{
    public TMP_InputField nameInput;
    
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    private void OnEnable()
    {
        //nameInput.text = "";
    }

    public void AddGame()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _gameDataProvider.AddGame(newName);
        nameInput.text = "";
    }
}
