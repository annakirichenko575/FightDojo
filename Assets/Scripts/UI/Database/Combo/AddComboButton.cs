using TMPro;
using UnityEngine;

public class AddComboButton : MonoBehaviour
{
    public TMP_InputField nameTMP;
    
    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
        _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
    }

    public void AddCombo()
    {
        string newName = nameTMP.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _comboDataProvider.AddCombo(newName);
        nameTMP.text = "";
    }
}