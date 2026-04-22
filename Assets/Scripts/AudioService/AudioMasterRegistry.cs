using FightDojo.AudioService;
using UnityEngine;

public class AudioMasterRegistry : MonoBehaviour
{
  [SerializeField] private AudioSource tickFx;
  [SerializeField] private AudioSource countdownFx;

  public void RegistryAudioSources(IAudioMasterService audioMaster)
  {
    audioMaster.RegistryAudioSources(tickFx, countdownFx);
  }
}
