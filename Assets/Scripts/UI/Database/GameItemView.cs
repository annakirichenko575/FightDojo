using TMPro;
using UnityEngine;

public class GameItemView : MonoBehaviour
{
    private TMP_Text _itemTMP;
    private int _id; 

    public void Initialize(int id, string gameName)
    {
        _id = id;
        _itemTMP = GetComponent<TMP_Text>();
        _itemTMP.text = $"{gameName}\n";
    }
}
