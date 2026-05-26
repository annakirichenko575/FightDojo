using FightDojo.Services;

namespace FightDojo.ComboEditor
{
  public interface ICurrentComboInfoService : IService
  {
    int ComboId { get; }
    string Author { get; }
    string Character { get; }
    string Game { get; }
    void UpdateComboInfo (int comboId, string author, string character, string game);
  }
}