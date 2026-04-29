using Services;
using UnityEngine;

namespace FightDojo.AudioService
{
  public interface IAudioMasterService : IService
  {
    void PlayTick();
    void RegistryAudioSources(AudioSource tickFx, AudioSource countdownFx);
    void PlayCountdown();
    void TickMute();
    void TickUnmute();
    bool IsTickMuted { get; }
  }
}