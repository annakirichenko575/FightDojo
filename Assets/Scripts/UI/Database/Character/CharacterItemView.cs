using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _background;
    
    private int _id;
    private CharacterDataProvider characterDataProvider;

    public int GetId => _id;

    public void Initialize(int id, string characterName, CharacterDataProvider characterDataProvider)
    {
        _id = id;
        _text.text = $"{characterName}\n";
        this.characterDataProvider = characterDataProvider;
        Unselect();
    }

    public void Highlight() =>
        _background.SetActive(true);

    public void Unselect() =>
        _background.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked CharacterItem id={_id}");
        characterDataProvider.SelectCharacter(_id);
    }
}