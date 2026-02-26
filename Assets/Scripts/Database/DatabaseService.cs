using Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FightDojo.Database
{
    interface IDatabaseService : IService
    {
        //перенести все методы в интерфейс
    }

    public class DatabaseService : IDatabaseService, IDisposable
    {
        public readonly string DbName = "FD.db";

        private SQLiteConnection _connection;
        private string _persistentDbPath;

        public DatabaseService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _persistentDbPath = Path.IsPathRooted(DbName) ? DbName : Path.Join(Application.persistentDataPath, DbName);
            //_persistentDbPath = Path.Combine(Application.persistentDataPath, "FD.db");

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

        public void AddCombo(Combos newCombo)
        {
            _connection.Insert(newCombo);
        }

        public void UpdateCombo(Combos combo)
        {
            _connection.Update(combo);
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

        // Вспомогательный класс для JOIN
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


        public void Dispose()
        {
            _connection?.Close();
            //_connection?.Dispose();
            _connection = null;
        }
    }
}