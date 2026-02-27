using TMPro;
using UnityEngine;

public class UpdateGameNameButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField nameInput;

    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    public void UpdateName()
    {
        // 1) проверяем id
        if (!int.TryParse(idInput.text, out int id))
        {
            Debug.LogWarning("Id должен быть числом!");
            return;
        }

        // 2) проверяем имя
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        // 3) обновляем в БД
        _gameDataProvider.UpdateGameName(id, newName);

        // очистить поля
        idInput.text = "";
        nameInput.text = "";
    }
}