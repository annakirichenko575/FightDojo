using Infrastructure.AssetManagement;
using UnityEngine;

namespace FightDojo.AudioService
{
  public class AudioMasterService : IAudioMasterService
  {
    private AudioSource tickFx;

    public AudioMasterService(IAssetProvider assetProvider)
    {
      AudioMasterRegistry audioMasterRegistry = 
        assetProvider.Instantiate(AssetPath.AudioMasterPath)
          .GetComponent<AudioMasterRegistry>();
      audioMasterRegistry.RegistryAudioSources(this);
    }
  
    public void RegistryAudioSources(AudioSource tickFx)
    {
      this.tickFx = tickFx;
    }
  
    public void PlayTick()
    {
      tickFx.Play();
    }
  
  }
}
