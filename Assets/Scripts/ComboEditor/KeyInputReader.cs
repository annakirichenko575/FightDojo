using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using FightDojo.Data;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem.LowLevel;

namespace FightDojo
{
    public class KeyInputReader
    {
        
        private float startTime = -1;
        private int nextItemId = 0;
        
        private float Now => Time.unscaledTime;
        
        private readonly Dictionary<string, string> ButtonMap 
            = new Dictionary<string, string>()
        {
            { Key.LeftArrow.ToString(), "left" },
            { Key.RightArrow.ToString(), "right" },
            { Key.UpArrow.ToString(), "up" },
            { Key.DownArrow.ToString(), "down" },
            { Key.Numpad0.ToString(), "num0" },
            { Key.Numpad1.ToString(), "num1" },
            { Key.Numpad2.ToString(), "num2" },
            { Key.Numpad3.ToString(), "num3" },
            { Key.Numpad4.ToString(), "num4" },
            { Key.Numpad5.ToString(), "num5" },
            { Key.Numpad6.ToString(), "num6" },
            { Key.Numpad7.ToString(), "num7" },
            { Key.Numpad8.ToString(), "num8" },
            { Key.Numpad9.ToString(), "num9" },
            
            { "buttonSouth", "padA" },     // A / Cross
            { "buttonEast",  "padB" },     // B / Circle
            { "buttonWest",  "padX" },     // X / Square
            { "buttonNorth", "padY" },     // Y / Triangle

            { "leftShoulder",  "L1" },
            { "rightShoulder", "R1" },

            { "leftTrigger",  "L2" },
            { "rightTrigger", "R2" },

            { "start",  "Start" },
            { "select", "Select" },

            { "leftStickPress",  "L3" },
            { "rightStickPress", "R3" },

            { "dpad/up",    "DUp" },
            { "dpad/down",  "DDown" },
            { "dpad/left",  "DLeft" },
            { "dpad/right", "DRight" },
        };
        
        // Буквенные клавиши A–Z
        private readonly Key[] LetterKeys =
        {
            Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
            Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
            Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
            Key.V, Key.W, Key.X, Key.Y, Key.Z
        };
        
        private readonly Key[] ArrowKeys =
        {
            Key.LeftArrow, Key.RightArrow, Key.UpArrow, Key.DownArrow
        };

        // Цифры справа (NumPad)
        private readonly Key[] NumpadKeys =
        {
            Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
            Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9
        };

        private List<Key> allKeys = new List<Key>();
        private float timeSpeed = 1f;
       
        public bool IsTimerStarted => startTime >= 0f;
        public float TimeSpeed => timeSpeed;
        
        // Инициализация таймера
        public KeyInputReader()
        {
            allKeys.AddRange(NumpadKeys);
            allKeys.AddRange(LetterKeys);
            allKeys.AddRange(ArrowKeys);
        }

        public float GetTimeLeft()
        {
            if (startTime < 0f)
                return 0f;

            return (Now - startTime) * timeSpeed;
        }
        
        public void Reset()
        {
            startTime = -1;
            nextItemId = 0;
        }

        // Проверяет указанный набор клавиш на press / release
        public KeyData CheckKeys(bool isTime = true)
        {
            foreach (var k in allKeys)
            {
                KeyControl key = Keyboard.current[k];
                if (key == null)
                    continue;

                if (TryGetButtonAction(key, out string action) == false)
                    continue;

                string keyName = GetKeyName(key.keyCode.ToString());

                float time = isTime ? GetLastInputTime() : 0f;

                KeyData keyData = new KeyData(nextItemId, action, time, keyName);
                nextItemId++;

                Debug.Log(keyData);

                return keyData;
            }

            Gamepad gamepad = Gamepad.current;
            if (gamepad != null)
            {
                foreach (var control in gamepad.allControls)
                {
                    if (control is ButtonControl button)
                    {
                        if (TryGetButtonAction(button, out string action) == false)
                            continue;
                        
                        string keyName = GetKeyName(button.name);
                        float time = isTime ? GetLastInputTime() : 0f;

                        KeyData keyData = new KeyData(nextItemId, action, time, keyName);
                        nextItemId++;

                        Debug.Log(keyData);

                        return keyData;
                    }
                }
            }
            
            return null;
        }

        private bool TryGetButtonAction(ButtonControl key, out string action)
        {
            action = null;
            if (key.wasPressedThisFrame)
                action = KeyData.PressedActionName;
            if (key.wasReleasedThisFrame)
                action = KeyData.ReleaseActionName;
            return action != null;
        }

        public void SpeedChanged(float speed)
        {
            timeSpeed = speed;
        }

        public void TimerStart()
        {
            if (IsTimerStarted == false)
            {
                startTime = Now;
            }
        }
    
        // Возвращает имя клавиши
        private string GetKeyName(string name)
        {
            if (ButtonMap.TryGetValue(name, out var mapped))
            {
                Debug.Log(mapped);
                return mapped;
            }
            else
            {
                Debug.Log("pad: " + name);
            } 
            return name;
        }

        // Логирует событие и задержку с предыдущего события
        private float GetLastInputTime()
        {
            TimerStart();
            return GetTimeLeft();
        }

    }
}
