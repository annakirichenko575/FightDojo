using System;
using System.Collections.Generic;
using Services;

namespace FightDojo.Database
{
    public interface IDatabaseService: IService, IDisposable
    {
        List<Game> GetAllGames();
        List<Character> GetCharactersByGame(int gameId);
        List<Combos> GetCombosByCharacter(int characterId);
        List<Combos> SearchCombosByTag(string tagFragment);
        int AddGame(Game game);
        void AddCombo(Combos newCombo);
        void UpdateCombo(Combos combo);
        void DeleteGame(int id);
        void DeleteCombo(int id);
        Combos GetCombo(int id);
        List<DatabaseService.ComboWithCharacter> GetCombosWithCharacterName(int gameId);
    }
}