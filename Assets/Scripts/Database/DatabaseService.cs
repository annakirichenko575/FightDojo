using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FightDojo.Database
{
    // C:/Users/Anna/AppData/LocalLow/DefaultCompany/FightDojo
    public class DatabaseService : IDatabaseService
    {
        private readonly string dbName = "FD.db";

        private SQLiteConnection _connection;
        private string _persistentDbPath;

        public DatabaseService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _persistentDbPath = Path.IsPathRooted(dbName) ? dbName : Path.Join(Application.persistentDataPath, dbName);
            bool dbExists = File.Exists(_persistentDbPath);
            try
            {
                _connection = new SQLiteConnection(_persistentDbPath);

                // создаём таблицы только если база была новой (или пустой)
                if (!dbExists)
                {
                    _connection.CreateTable<Game>();
                    _connection.CreateTable<Character>();
                    _connection.CreateTable<Combos>();
                    Debug.Log("Создана новая база данных: " + _persistentDbPath);
                }
                else
                {
                    // можно добавить проверку наличия таблиц, если нужно
                    Debug.Log("База уже существует: " + _persistentDbPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка инициализации базы: " + ex.Message);
            }
        }

        public List<Game> GetAllGames() //перенести методы в интерфейс
        {
            return _connection.Table<Game>().ToList();
        }

        public List<Character> GetCharactersByGame(int gameId)
        {
            return _connection.Table<Character>()
                .Where(c => c.GameId == gameId)
                .ToList();
        }

        public List<Combos> GetCombosByCharacter(int characterId)
        {
            return _connection.Table<Combos>()
                .Where(c => c.CharacterId == characterId)
                .ToList();
        }

        public List<Combos> SearchCombosByTag(string tagFragment)
        {
            // Пример простого поиска по подстроке в tags
            return _connection.Table<Combos>()
                .Where(c => c.Tags.Contains(tagFragment))
                .ToList();
        }

        public int AddGame(Game newGame)
        {
            return _connection.Insert(newGame);
        }

        public void AddCombo(Combos newCombo)
        {
            _connection.Insert(newCombo);
        }

        public void UpdateCombo(Combos combo)
        {
            _connection.Update(combo);
        }

        public void DeleteGame(int id)
        {
            _connection.Delete<Game>(id);
        }

        public void DeleteCombo(int id)
        {
            _connection.Delete<Combos>(id);
        }

        // Получить одно комбо по id
        public Combos GetCombo(int id)
        {
            return _connection.Find<Combos>(id);
        }

        // Пример JOIN (raw SQL)
        public List<ComboWithCharacter> GetCombosWithCharacterName(int gameId)
        {
            string sql = @"
                SELECT comb.*, char.name AS CharacterName
                FROM Combo comb
                INNER JOIN Character char ON comb.CharacterId = char.Id
                WHERE char.GameId = ?
                ORDER BY comb.Id DESC";

            return _connection.Query<ComboWithCharacter>(sql, gameId);
        }

        public void UpdateGameName(int id, string newName)
        {
            var game = _connection.Find<Game>(id);
            if (game == null)
            {
                Debug.LogWarning($"Игра с id={id} не найдена");
                return;
            }

            game.Name = newName;
            _connection.Update(game);

            Debug.Log($"Имя игры обновлено: id={id}, newName={newName}");
        }
        
        public void Dispose()
        {
            _connection?.Close();
            _connection = null;
        }

        public class ComboWithCharacter
        {
            public int Id { get; set; }
            public int CharacterId { get; set; }
            public string Combo { get; set; }
            public string CreatorName { get; set; }
            public string Description { get; set; }
            public string Tags { get; set; }
            public string CharacterName { get; set; }
        }
        
    }
}