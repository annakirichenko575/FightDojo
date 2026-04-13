using TMPro;
using UnityEngine;

namespace FightDojo
{
  public class TimelinePartView : MonoBehaviour
  {
    [SerializeField] private TMP_Text _time;

    public void SetText(string text)
    {
      _time.text = text;
    }
  }
  
}
