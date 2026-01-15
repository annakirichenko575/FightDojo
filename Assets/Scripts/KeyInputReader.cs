using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using FightDojo.Data;

namespace FightDojo
{
    public class KeyInputReader
    {
        private Stopwatch stopwatch;
        private long lastEventMs = -1;

        // Буквенные клавиши A–Z
        private readonly Key[] LetterKeys =
        {
            Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
            Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
            Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
            Key.V, Key.W, Key.X, Key.Y, Key.Z
        };

        // Цифры справа (NumPad)
        private readonly Key[] NumpadKeys =
        {
            Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
            Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9
        };

        private List<Key> AllKeys = new List<Key>();

        // Инициализация таймера
        public KeyInputReader()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            AllKeys.AddRange(NumpadKeys);
            AllKeys.AddRange(LetterKeys);
        }

        public void Reset()
        {
            stopwatch.Restart();
            lastEventMs = -1;
        }

        // Проверяет указанный набор клавиш на press / release
        public KeyData CheckKeys()
        {
            foreach (var k in AllKeys)
            {
                var key = Keyboard.current[k];
                if (key == null)
                    continue;

                string action = null;
                if (key.wasPressedThisFrame)
                    action = "press";
                if (key.wasReleasedThisFrame)
                    action = "release";

                if (action == null)
                    continue;

                string keyName = GetKeyName(key);
                long delta = RecountDeltaLastInput();
                KeyData keyData = new KeyData(action, delta, keyName);

                UnityEngine.Debug.Log(keyData);

                return keyData;
            }
            return null;
        }

        // Возвращает имя клавиши
        private string GetKeyName(KeyControl key)
        {
            return key.keyCode.ToString();
        }

        // Логирует событие и задержку с предыдущего события
        private long RecountDeltaLastInput()
        {
            long now = stopwatch.ElapsedMilliseconds;
            bool first = lastEventMs < 0;
            long delta = first ? 0 : now - lastEventMs;
            lastEventMs = now;
            return delta;
        }
    }
}
