using TMPro;
using UnityEngine;

public class AddCharacterButton : MonoBehaviour
{
    public TMP_InputField nameTMP;
    
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void AddCharacter()
    {
        string newName = nameTMP.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _gameDataProvider.AddCharacter(newName);
        nameTMP.text = "";
    }
}