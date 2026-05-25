using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public string DatabasePath { get; private set; }

        public DatabaseService()
        {
            TryInitializeDatabase();
        }

        private bool TryInitializeDatabase()
        {
            _persistentPath = Path.IsPathRooted(dbName) 
                ? dbName 
                : Path.Combine(Application.persistentDataPath, dbName);
            return TryInitializeDatabase(_persistentPath);
        }

        private bool TryInitializeDatabase(string dbPath)
        {
            dbPath = Path.GetFullPath(dbPath);
            bool dbExists = File.Exists(dbPath);
            try
            {
                SQLiteConnection newConnection = new SQLiteConnection(dbPath);

                if (dbExists)
                {
                    bool valid = ValidateTable<Game>(newConnection)
                                 && ValidateTable<Character>(newConnection)
                                 && ValidateTable<Combos>(newConnection);

                    if (!valid)
                        throw new Exception("Схема базы данных не совпадает с ожидаемой");
                    
                    Debug.Log("База уже существует: " + dbPath);
                }
                else
                {
                    newConnection.CreateTable<Game>();
                    newConnection.CreateTable<Character>();
                    newConnection.CreateTable<Combos>();
                    Debug.Log("Создана новая база данных: " + dbPath);
                }

                _connection = newConnection;
                DatabasePath = dbPath;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка инициализации базы: " + ex.Message);
                return false;
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
       
        public bool TryMergeDatabases(string secondDbPath)
        {
            bool isSuccess = true;
            _connection.Execute("PRAGMA foreign_keys = OFF;");

            try
            {
                secondDbPath = Path.GetFullPath(secondDbPath);
                bool dbExists = File.Exists(secondDbPath);

                if (dbExists == false)
                    throw new Exception("Импортируемый файл не найден");

                using (SQLiteConnection newConnection = new SQLiteConnection(secondDbPath))
                {
                    bool valid = ValidateTable<Game>(newConnection)
                                 && ValidateTable<Character>(newConnection)
                                 && ValidateTable<Combos>(newConnection);

                    if (valid == false)
                        throw new Exception("Схема импортируемой базы данных не совпадает с ожидаемой");
                }

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
                isSuccess = false;
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
            return isSuccess;
        }

        public bool TryOpenDatabase(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Debug.LogError("OpenDatabase: path is empty");
                return false;
            }

            SQLiteConnection tempConnection = _connection;
            if (TryInitializeDatabase(path))
            {
                tempConnection?.Close();
                Debug.Log($"База данных успешно открыта → {path} ");
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection = null;
        }
        
        private bool ValidateTable<T>(SQLiteConnection connection)
        {
            var map = connection.GetMapping<T>();
            var tableName = map.TableName;

            // получаем реальные колонки из базы
            var cols = connection
                .Query<TableInfo>($"PRAGMA table_info({tableName})")
                .Select(c => c.name)
                .ToHashSet();

            if (cols.Count == 0)
            {
                Debug.LogError($"Таблица '{tableName}' отсутствует в базе");
                return false;
            }

            // проверяем что все колонки модели присутствуют
            foreach (var col in map.Columns)
            {
                if (!cols.Contains(col.Name))
                {
                    Debug.LogError($"'{tableName}': колонка '{col.Name}' отсутствует");
                    return false;
                }
            }

            return true;
        }

        // вспомогательный класс для PRAGMA
        private class TableInfo
        {
            public string name { get; set; }
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