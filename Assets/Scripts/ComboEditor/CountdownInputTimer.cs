using FightDojo.AudioService;
using UnityEngine;

namespace FightDojo
{
  public class CountdownInputTimer 
  {
    private readonly KeyInputReader keyInputReader;
    private readonly IAudioMasterService audioMaster;
    private readonly int currentTickParts = 4;
        
    private float countdownTime;
    private int currentTick;
    private float maxCountdownTime = 2f;
        
    float CountdownTimePart => maxCountdownTime / currentTickParts * currentTick;
        
    public CountdownInputTimer(KeyInputReader keyInputReader, IAudioMasterService audioMaster)
    {
      this.keyInputReader = keyInputReader;
      this.audioMaster = audioMaster;
    }

    public void Tick()
    {
      if (keyInputReader.IsTimerStarted == false 
          && currentTick < currentTickParts + 1f)
      {
        countdownTime += Time.deltaTime * keyInputReader.TimeSpeed;
        if (countdownTime > CountdownTimePart)
        {
          if (currentTick < currentTickParts)
          {
            currentTick++;
            audioMaster.PlayCountdown();
          }
          else
          {
            keyInputReader.TimerStart();
          } 
        }
      }
    }

    public void Reset()
    {
      countdownTime = 0;
      currentTick = 0;
    }

    public void MaxTimeChanged(float maxTime)
    {
      maxCountdownTime = maxTime;
    }
  }
}