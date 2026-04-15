using TMPro;
using UnityEngine;

namespace FightDojo
{
  public class TimelinePartView : MonoBehaviour
  {
    [SerializeField] private TMP_Text _time;

    public void SetTime(int time)
    {
      _time.text = $"{time}:00";
    }
  }
  
}
