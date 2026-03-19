using TMPro;
using UnityEngine;

public class AddCharacterButton : MonoBehaviour
{
    public TMP_InputField nameInput;
    
    private CharacterDataProvider _characterDataProvider;

    private void Awake()
    {
        _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    private void OnEnable()
    {
        //nameInput.text = "";
    }

    public void AddCharacter()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _characterDataProvider.AddCharacter(newName);
        nameInput.text = "";
    }
}