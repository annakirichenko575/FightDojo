using TMPro;
using UnityEngine;

public class AddComboButton : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField descriptionInput;
    public TMP_InputField tagsInput;
    
    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
        _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
    }

    private void OnEnable()
    {
        //nameInput.text = "";
    }

    public void AddCombo()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }
        
        _comboDataProvider.AddCombo(newName, descriptionInput.text, tagsInput.text);
        nameInput.text = "";
        descriptionInput.text = "";
        tagsInput.text = "";
    }
}