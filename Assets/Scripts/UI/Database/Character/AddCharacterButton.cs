using TMPro;
using UnityEngine;

public class AddCharacterButton : MonoBehaviour
{
    public TMP_InputField nameTMP;
    
    private CharacterDataProvider _characterDataProvider;

    private void Awake()
    {
        _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    public void AddCharacter()
    {
        string newName = nameTMP.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _characterDataProvider.AddCharacter(newName);
        nameTMP.text = "";
    }
}