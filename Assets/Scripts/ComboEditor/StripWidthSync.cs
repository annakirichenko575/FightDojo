using UnityEngine;

namespace FightDojo
{
  public class StripWidthSync
  {
    private IStripWidth _stripWidthReader1;
    private IStripWidth _stripWidthReader2;

    public void InitializeStrip(IStripWidth stripWidth)
    {
      if (_stripWidthReader1 == null)
      {
        _stripWidthReader1 = stripWidth;
      }
      else if (_stripWidthReader2 == null)
      {
        _stripWidthReader2 = stripWidth;
      }
    }
    
    public float GetMaxWidth() =>
      Mathf.Max(_stripWidthReader1.GetCurrentWidth(),
        _stripWidthReader2.GetCurrentWidth());

    public void SyncWidth()
    {
      _stripWidthReader1.UpdateContentWidth();
      _stripWidthReader2.UpdateContentWidth();
    }
  }
}