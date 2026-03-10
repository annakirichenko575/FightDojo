using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _background;
    
    private int _id;
    private GameDataProvider gameDataProvider;

    public int GetId => _id;

    public void Initialize(int id, string characterName, GameDataProvider gameDataProvider)
    {
        _id = id;
        _text.text = $"{characterName}\n";
        this.gameDataProvider = gameDataProvider;
        Unselect();
    }

    public void Highlight() =>
        _background.SetActive(true);

    public void Unselect() =>
        _background.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked CharacterItem id={_id}");
        gameDataProvider.SelectCharacter(_id);
    }
}