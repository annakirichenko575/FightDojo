using TMPro;
using FightDojo.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComboItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _background;
    
    private int _id;
    private ComboDataProvider comboDataProvider;

    public int GetId => _id;

    public void Initialize(int id, string creatorName, string description, string tags, ComboDataProvider comboDataProvider)
    {
        _id = id;
        Debug.Log(creatorName + ", " + description + ", " + tags);
        _text.text = $"{ShortText(creatorName, 22)} " +
                     $"{ShortText(description,  40)} " +
                     $"{ShortText(tags,  23)}\n";
        this.comboDataProvider = comboDataProvider;
        Unselect();
    }

    public void Highlight() =>
        _background.SetActive(true);

    public void Unselect() =>
        _background.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        // eventData.clickCount автоматически считает клики в пределах времени двойного клика Unity
        if (eventData.clickCount == 2 
            && eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Двойной клик!");
            OnDoubleClick();
        }
        else if (eventData.clickCount == 1)
        {
            Debug.Log("Одиночный клик");
            Debug.Log($"Clicked ComboItem id={_id}");
            comboDataProvider.SelectCombo(_id);
        } 
    }

    private void OnDoubleClick()
    {
        CanvasRoots canvasRoots = FindAnyObjectByType<CanvasRoots>();
        canvasRoots.OpenComboCanvas();
    }

    private string ShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
        {
            return text;
        }
        
        Debug.Log("Sub");
        string sub = text.Substring(0, maxLength);
        Debug.Log(sub);
        string split = sub.Split('\n')[0].TrimEnd('\r');
        Debug.Log(split);
        
        return split;
    }
}