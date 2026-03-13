using TMPro;
using UnityEngine;

public class UpdateComboButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;

    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
        _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
    }

    public void UpdateCombo()
    {
        string newName = nameInput.text;
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Новое имя не введено!");
            return;
        }

        _comboDataProvider.UpdateCombo(newName);
        nameInput.text = "";
    }
}