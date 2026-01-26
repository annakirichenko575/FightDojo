using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using FightDojo.Data;
using UnityEngine;

namespace FightDojo
{
    public class KeyInputReader
    {
        
        private float startTime = -1;
        private int nextItemId = 0;
        

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
            AllKeys.AddRange(NumpadKeys);
            AllKeys.AddRange(LetterKeys);
        }

        public void Reset()
        {
            startTime = -1;
            nextItemId = 0;
        }

        // Проверяет указанный набор клавиш на press / release
        public KeyData CheckKeys(bool isTime = true)
        {
            foreach (var k in AllKeys)
            {
                var key = Keyboard.current[k];
                if (key == null)
                    continue;

                string action = null;
                if (key.wasPressedThisFrame)
                    action = KeyData.IsPressedAction;
                if (key.wasReleasedThisFrame)
                    action = KeyData.IsReleaseAction;

                if (action == null)
                    continue;

                string keyName = GetKeyName(key);

                float time = isTime ? GetLastInputTime() : 0f;

                KeyData keyData = new KeyData(nextItemId, action, time, keyName);
                nextItemId++;

                Debug.Log(keyData);

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
        private float GetLastInputTime()
        {
            float now = Time.unscaledTime;
            if (startTime < 0f)
            {
                startTime = now;
            }
            return now - startTime;
        }
    }
}
