using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _background;
    
    private int _id;
    private GameDataProvider gameDataProvider;

    public int GetId => _id;

    public void Initialize(int id, string gameName, GameDataProvider gameDataProvider)
    {
        _id = id;
        _text.text = $"{gameName}\n";
        this.gameDataProvider = gameDataProvider;
        Unselect();
    }

    public void Highlight() => 
        _background.SetActive(true);

    public void Unselect() => 
        _background.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked Game id={_id}");
        gameDataProvider.SelectGame(_id);
    }
}
