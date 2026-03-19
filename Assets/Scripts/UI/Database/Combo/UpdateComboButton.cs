using FightDojo.Database;
using TMPro;
using UnityEngine;

public class UpdateComboButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField descriptionInput;
    [SerializeField] private TMP_InputField tagsInput;
    
    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
        _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
    }

    private void OnEnable()
    {
        _comboDataProvider.CurrentCombo(out Combos comboData);
        nameInput.text = comboData.CreatorName;
        descriptionInput.text = comboData.Description;
        tagsInput.text = comboData.Tags;
    }

    public void UpdateCombo()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        _comboDataProvider.UpdateCombo(newName, descriptionInput.text, tagsInput.text);
        nameInput.text = "";
        descriptionInput.text = "";
        tagsInput.text = "";
    }
}