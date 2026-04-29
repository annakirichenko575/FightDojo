using FightDojo.AudioService;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace FightDojo.UI
{
  [RequireComponent(typeof(Image))]
  public class RhytmButton : MonoBehaviour
  {
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;
  
    private Image _image;
    private IAudioMasterService _audioMasterService;

    private void Awake()
    {
      _image = GetComponent<Image>();
      _audioMasterService = AllServices.Container.Single<IAudioMasterService>();
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
      if (_audioMasterService.IsTickMuted)
      {
        _audioMasterService.TickUnmute();
      }
      else
      {
        _audioMasterService.TickMute();
      }
    }

    private void UpdateImage() => 
      _image.sprite = _audioMasterService.IsTickMuted 
        ? _soundOff : _soundOn;
  }
}
