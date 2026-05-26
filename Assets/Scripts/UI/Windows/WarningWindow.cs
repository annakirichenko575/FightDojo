using TMPro;
using UnityEngine;

namespace FightDojo.UI.Windows
{
  public class WarningWindow : MonoBehaviour
  {
    [SerializeField] private GameObject _wrap;
    [SerializeField] private GameObject _waningPanel;
    [SerializeField] private TMP_Text _warning;

    private void Start()
    {
      Hide();
    }

    public void OpenWarning(string warningText)
    {
      CloseAllWindows();
      _warning.text = warningText;
      _waningPanel.SetActive(true);
      Show();
    }

    public void Hide()
    {
      CloseAllWindows();
      _wrap.SetActive(false);
    }

    private void Show()
    {
      _wrap.SetActive(true);
    }

    private void CloseAllWindows()
    {
      _waningPanel.SetActive(false);
    }
  }
}