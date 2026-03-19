using System;
using FightDojo.Database;
using TMPro;
using UnityEngine;

public class UpdateGameNameButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindAnyObjectByType<GameDataProvider>();
    }

    private void OnEnable()
    {
        _gameDataProvider.CurrentGame(out Game gameData);
        nameInput.text = gameData.Name;
    }

    public void UpdateName()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        // 3) обновляем в БД
        _gameDataProvider.UpdateGameName(newName);

        // очистить поля
        nameInput.text = "";
    }
}