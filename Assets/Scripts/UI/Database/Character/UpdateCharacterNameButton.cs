using TMPro;
using UnityEngine;

public class UpdateCharacterNameButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private CharacterDataProvider _characterDataProvider;

    private void Awake()
    {
        _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    public void UpdateName()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        _characterDataProvider.UpdateCharacterName(newName);
        nameInput.text = "";
    }
}