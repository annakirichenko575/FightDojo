namespace FightDojo.Services.Randomizer
{
  public interface IRandomService : IService
  {
    int Next(int minValue, int maxValue);
  }
}