using System;
using UnityEngine;

public class GameWindow : MonoBehaviour
{
    [SerializeField] private GameObject _wrap;
    [SerializeField] private GameObject _addWindow;
    [SerializeField] private GameObject _updateWindow;
    [SerializeField] private GameObject _deleteWindow;
    [SerializeField] private GameObject _warningDeleteWindow;
    private CharacterDataProvider _characterDataProvider;
    private GameDataProvider _gameDataProvider;

    private void Awake()
    {
        _gameDataProvider = FindObjectOfType<GameDataProvider>();
        _characterDataProvider = FindAnyObjectByType<CharacterDataProvider>();
    }

    private void Start()
    {
        Hide();
    }

    public void OpenAddWindow()
    {
        CloseAllWindows();
        _addWindow.SetActive(true);
        Show();
    }

    public void OpenUpdateWindow()
    {
        if (_gameDataProvider.HasSelectedGame == false)
            return;
        
        CloseAllWindows();
        _updateWindow.SetActive(true);
        Show();
    }
    
    public void OpenDeleteWindow()
    {
        if (_gameDataProvider.HasSelectedGame == false)
            return;
        
        CloseAllWindows();
        if (_characterDataProvider.Count > 0)
        {
            _warningDeleteWindow.SetActive(true);
        }
        else
        {
            _deleteWindow.SetActive(true);
        }
        Show();
    }

    public void Hide()
    {
        CloseAllWindows();
        _wrap.SetActive(false);
    }
    
    private void Show()
    {
        _wrap.SetActive(true);
    }

    private void CloseAllWindows()
    {
        _warningDeleteWindow.SetActive(false);
        _deleteWindow.SetActive(false);
        _addWindow.SetActive(false);
        _updateWindow.SetActive(false);
    }
}
