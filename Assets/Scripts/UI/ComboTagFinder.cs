using System;
using TMPro;
using UnityEngine;

namespace FightDojo.UI
{
  [RequireComponent(typeof(TMP_InputField))]
  public class ComboTagFinder : MonoBehaviour
  {
    private TMP_InputField _inputField;
    private ComboDataProvider _comboDataProvider;

    private void Awake()
    {
      _comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
      _inputField = GetComponent<TMP_InputField>();
      //_inputField.onValueChanged.AddListener(Find);
      _inputField.onEndEdit.AddListener(Find);
    }

    private void Find(string tags)
    {
      if (string.IsNullOrEmpty(tags))
      {
        _comboDataProvider.RefreshCombos();
        return;
      }
        
      _comboDataProvider.FindByTags(tags);
    }
  }
}
