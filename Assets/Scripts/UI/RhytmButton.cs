using FightDojo.ComboEditor.AudioService;
using FightDojo.Services;
using UnityEngine;
using UnityEngine.UI;

namespace FightDojo.UI
{
  [RequireComponent(typeof(Image))]
  public class RhytmButton : MonoBehaviour
  {
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;
  
    private Image image;
    private IAudioMasterService audioMasterService;

    private void Awake()
    {
      image = GetComponent<Image>();
      audioMasterService = AllServices.Container.Single<IAudioMasterService>();
    }

    private void Start()
    {
      UpdateImage();
    }

    public void Apply()
    {
      ToggleTickMute();
      UpdateImage();
    }

    private void ToggleTickMute()
    {
      if (audioMasterService.IsTickMuted)
      {
        audioMasterService.TickUnmute();
      }
      else
      {
        audioMasterService.TickMute();
      }
    }

    private void UpdateImage() => 
      image.sprite = audioMasterService.IsTickMuted 
        ? _soundOff : _soundOn;
  }
}
