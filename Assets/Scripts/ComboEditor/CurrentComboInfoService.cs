namespace FightDojo.ComboEditor
{
  public class CurrentComboInfoService : ICurrentComboInfoService
  {
    public int ComboId { get; private set; }
    public string Author { get; private set; }
    public string Character { get; private set;}
    public string Game { get; private set;}

    public void UpdateComboInfo (int comboId, string author, string character, string game)
    {
      ComboId = comboId;
      Author = author;
      Character = character;
      Game = game;
    }
  }
}