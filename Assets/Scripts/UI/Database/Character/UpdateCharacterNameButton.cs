using TMPro;
using UnityEngine;

public class UpdateCharacterNameButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void UpdateName()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        _gameDataProvider.UpdateCharacterName(newName);
        nameInput.text = "";
    }
}