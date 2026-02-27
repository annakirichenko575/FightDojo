using System;
using SQLite;

namespace FightDojo.Database
{
    [Serializable]
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }
    }

    [Serializable]
    public class Character
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int GameId { get; set; }

        [NotNull]
        public string Name { get; set; }
    }
    
    [Serializable]
    public class Combos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int CharacterId { get; set; }

        public string Combo { get; set; }          // json или строка с комбо
        public string CreatorName { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }           // например "#easy;#corner"
    }
}