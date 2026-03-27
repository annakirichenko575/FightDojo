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
        private string _persistentPath;

        public string PersistentPath => Application.persistentDataPath;

        public DatabaseService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _persistentPath = Path.IsPathRooted(dbName) ? dbName : Path.Join(Application.persistentDataPath, dbName);
            InitializeDatabase(_persistentPath);
        }

        private void InitializeDatabase(string dbPath)
        {
            bool dbExists = File.Exists(dbPath);
            try
            {
                _connection = new SQLiteConnection(dbPath);

                // создаём таблицы только если база была новой (или пустой)
                if (!dbExists)
                {
                    _connection.CreateTable<Game>();
                    _connection.CreateTable<Character>();
                    _connection.CreateTable<Combos>();
                    Debug.Log("Создана новая база данных: " + dbPath);
                }
                else
                {
                    // можно добавить проверку наличия таблиц, если нужно
                    Debug.Log("База уже существует: " + dbPath);
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
        
        public int AddCharacter(Character newCharacter)
        {
            return _connection.Insert(newCharacter);
        }

        public int AddCombo(Combos newCombo)
        {
            return _connection.Insert(newCombo);
        }

        public void DeleteGame(int id)
        {
            _connection.Delete<Game>(id);
        }

        public void DeleteCharacter(int id)
        {
            _connection.Delete<Character>(id);
        }
        
        public void DeleteCombo(int id)
        {
            _connection.Delete<Combos>(id);
        }

        public Game GetGame(int id) => 
            _connection.Find<Game>(id);

        public Character GetCharacter(int id) => 
            _connection.Find<Character>(id);

        public Combos GetCombo(int id) => 
            _connection.Find<Combos>(id);

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
        
        public void UpdateCharacterName(int id, string newName)
        {
            var character = _connection.Find<Character>(id);
            if (character == null)
            {
                Debug.LogWarning($"Перс с id={id} не найдена");
                return;
            }

            character.Name = newName;
            _connection.Update(character);

            Debug.Log($"Имя перса обновлено: id={id}, newName={newName}");
        }
        
        public void UpdateComboJson(int id, string comboJson)
        {
            var combo = _connection.Find<Combos>(id);
            if (combo == null)
            {
                Debug.LogWarning($"Комбо с id={id} не найдена");
                return;
            }

            combo.Combo = comboJson;
            _connection.Update(combo);

            Debug.Log($"Json комбо обновлено: id={id}");
        }

        public void UpdateCombo(int id, string newName, string newDesc, string newTags)
        {
            var combo = _connection.Find<Combos>(id);
            if (combo == null)
            {
                Debug.LogWarning($"Комбо с id={id} не найдена");
                return;
            }

            combo.CreatorName = newName;
            combo.Description = newDesc;
            combo.Tags = newTags;
            _connection.Update(combo);

            Debug.Log($"Имя создателя комбо обновлено: id={id}, newName={newName}");
        }
       
        public bool ExportDatabase(string exportPath)
        {
            if (_connection == null)
            {
                Debug.LogError("ExportDatabase: connection is null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(exportPath))
            {
                Debug.LogError("ExportDatabase: exportPath is empty");
                return false;
            }

            try
            {
                // Самый надёжный способ — сериализация всей базы в байты
                byte[] dbBytes = _connection.Serialize();

                // Записываем в файл
                File.WriteAllBytes(exportPath, dbBytes);

                Debug.Log($"База данных успешно экспортирована → {exportPath} ({dbBytes.Length} байт)");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при экспорте базы в {exportPath}\n{ex.Message}\n{ex.StackTrace}");
                return false;
            }
        } 
       
        public void MergeDatabases(string secondDbPath)
        {
            _connection.Execute("PRAGMA foreign_keys = OFF;");

            try
            {
                _connection.Execute("ATTACH DATABASE ? AS db2;", secondDbPath);

                _connection.RunInTransaction(() =>
                {
                    // Добавляем временные колонки для старых Id
                    try
                    {
                        _connection.Execute("ALTER TABLE Game ADD COLUMN OldId INTEGER;");
                    }
                    catch
                    {
                        // Уже существует
                    }

                    try
                    {
                        _connection.Execute("ALTER TABLE Character ADD COLUMN OldId INTEGER;");
                    }
                    catch
                    {
                        // Уже существует
                    }

                    // Копируем Game + сохраняем старый Id
                    _connection.Execute(@"
                        INSERT INTO Game (Name, OldId)
                        SELECT Name, Id
                        FROM db2.Game;
                    ");

                    // Копируем Character + сохраняем старый Id
                    _connection.Execute(@"
                        INSERT INTO Character (Name, GameId, OldId)
                        SELECT c.Name, g.Id, c.Id
                        FROM db2.Character c
                        JOIN Game g ON g.OldId = c.GameId;
                    ");

                    // Копируем Combos с правильным CharacterId
                    _connection.Execute(@"
                        INSERT INTO Combos (CharacterId, Combo, CreatorName, Description, Tags)
                        SELECT ch.Id, cb.Combo, cb.CreatorName, cb.Description, cb.Tags
                        FROM db2.Combos cb
                        JOIN Character ch ON ch.OldId = cb.CharacterId;
                    ");

                    // Удаляем временные колонки
                    _connection.Execute("ALTER TABLE Game DROP COLUMN OldId;");
                    _connection.Execute("ALTER TABLE Character DROP COLUMN OldId;");
                });
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                try
                {
                    _connection.Execute("DETACH DATABASE db2;");
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            //_connection.Execute("PRAGMA foreign_keys = ON;");
        }

        public bool OpenDatabase(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogError("OpenDatabase: path is empty");
                return false;
            }

            try
            {
                SQLiteConnection tempConnection = _connection;
                InitializeDatabase(path);
                if (tempConnection != null)
                {
                    tempConnection.Close();
                }
                Debug.Log($"База данных успешно открыта → {path} ");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при открытии базы в {path}\n{ex.Message}\n{ex.StackTrace}");
                return false;
            } 
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