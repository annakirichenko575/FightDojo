using FightDojo.Database;
using TMPro;
using UnityEngine;

namespace FightDojo.UI
{
  [RequireComponent(typeof(TMP_InputField))]
  public class ComboTagFinder : MonoBehaviour
  {
    private TMP_InputField inputField;
    private ComboDataProvider comboDataProvider;

    private void Awake()
    {
      comboDataProvider = FindAnyObjectByType<ComboDataProvider>();
      inputField = GetComponent<TMP_InputField>();
      //_inputField.onValueChanged.AddListener(Find);
      inputField.onEndEdit.AddListener(Find);
    }

    private void Find(string tags)
    {
      if (string.IsNullOrEmpty(tags))
      {
        comboDataProvider.RefreshCombos();
        return;
      }
        
      comboDataProvider.FindByTags(tags);
    }
  }
}
