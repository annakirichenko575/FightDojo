using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class KeyTimingLogger : MonoBehaviour
{
    private Stopwatch stopwatch;
    private long lastEventMs = -1;

    // Буквенные клавиши A–Z
    private static readonly Key[] LetterKeys =
    {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
        Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
        Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
        Key.V, Key.W, Key.X, Key.Y, Key.Z
    };

    // Цифры справа (NumPad)
    private static readonly Key[] NumpadKeys =
    {
        Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
        Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9
    };

    // Инициализация таймера
    private void Awake()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    // Проверка нажатий и отпусканий клавиш каждый кадр
    private void Update()
    {
        if (Keyboard.current == null)
            return;

        CheckKeys(LetterKeys);
        CheckKeys(NumpadKeys);
    }

    // Проверяет указанный набор клавиш на press / release
    private void CheckKeys(Key[] keys)
    {
        foreach (var k in keys)
        {
            var key = Keyboard.current[k];
            if (key == null)
                continue;

            if (key.wasPressedThisFrame)
                LogEvent(GetKeyName(key), "press");

            if (key.wasReleasedThisFrame)
                LogEvent(GetKeyName(key), "release");
        }
    }

    // Возвращает имя клавиши
    private string GetKeyName(UnityEngine.InputSystem.Controls.KeyControl key)
    {
        return key.keyCode.ToString();
    }

    // Логирует событие и задержку с предыдущего события
    private void LogEvent(string keyName, string action)
    {
        long now = stopwatch.ElapsedMilliseconds;
        bool first = lastEventMs < 0;
        long delta = first ? 0 : now - lastEventMs;

        lastEventMs = now;

        if (first)
            UnityEngine.Debug.Log($"[FIRST] [{action}] key={keyName} delta_ms={delta}");
        else
            UnityEngine.Debug.Log($"[{action}] key={keyName} delta_ms={delta}");
    }
}
