using FightDojo.AudioService;
using UnityEngine;

public class AudioMasterRegistry : MonoBehaviour
{
  [SerializeField] private AudioSource tickFx;

  public void RegistryAudioSources(IAudioMasterService audioMaster)
  {
    audioMaster.RegistryAudioSources(tickFx);
  }
}
