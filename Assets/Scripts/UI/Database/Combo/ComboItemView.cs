using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _background;
    
    private int _id;
    private ComboDataProvider comboDataProvider;

    public int GetId => _id;

    public void Initialize(int id, string creatorName, ComboDataProvider comboDataProvider)
    {
        _id = id;
        _text.text = $"{creatorName}\n";
        this.comboDataProvider = comboDataProvider;
        Unselect();
    }

    public void Highlight() =>
        _background.SetActive(true);

    public void Unselect() =>
        _background.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked ComboItem id={_id}");
        comboDataProvider.SelectCombo(_id);
    }
}