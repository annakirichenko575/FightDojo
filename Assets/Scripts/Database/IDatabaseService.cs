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
        int AddGame(Game newGame);
        int AddCharacter(Character newCharacter);
        int AddCombo(Combos newCombo);
        void UpdateGameName(int id, string newName);
        void UpdateCharacterName(int id, string newName);
        void UpdateCombo(int id, string newName, string newDesc, string newTags);
        void DeleteGame(int id);
        void DeleteCharacter(int id);
        void DeleteCombo(int id);
        Combos GetCombo(int id);
        Game GetGame(int id);
        Character GetCharacter(int id);
        List<DatabaseService.ComboWithCharacter> GetCombosWithCharacterName(int gameId);
    }
}