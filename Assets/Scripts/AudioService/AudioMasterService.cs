using Infrastructure.AssetManagement;
using UnityEngine;

namespace FightDojo.AudioService
{
  public class AudioMasterService : IAudioMasterService
  {
    private AudioSource tickFx;
    private AudioSource countdownFx;
    private bool isTickMute = false;
    
    public bool IsTickMuted => isTickMute;

    public AudioMasterService(IAssetProvider assetProvider)
    {
      AudioMasterRegistry audioMasterRegistry = 
        assetProvider.Instantiate(AssetPath.AudioMasterPath)
          .GetComponent<AudioMasterRegistry>();
      audioMasterRegistry.RegistryAudioSources(this);
    }
  
    public void RegistryAudioSources(AudioSource tickFx, AudioSource countdownFx)
    {
      this.tickFx = tickFx;
      this.countdownFx = countdownFx;
    }
  
    public void PlayTick()
    {
      if (isTickMute)
        return;
      
      tickFx.Play();
    }

    public void PlayCountdown()
    {
      countdownFx.Play();
    }

    public void TickMute() => isTickMute = true;

    public void TickUnmute() => isTickMute = false;
  }
}
