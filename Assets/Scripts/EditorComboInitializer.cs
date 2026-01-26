using System;
using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace FightDojo
{
    public class EditorComboInitializer : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float stripScale = 2000f;

        private RecordedKeys recordedKeys;
        private EditorComboBuilder comboBuilder;
        private StripItemView currentStripItemView = null;

        // Разрешённые клавиши: буквы A–Z
        private static readonly Key[] LetterKeys =
        {
            Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
            Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
            Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
            Key.V, Key.W, Key.X, Key.Y, Key.Z
        };

        // Разрешённые клавиши: цифры справа (NumPad)
        private static readonly Key[] NumpadKeys =
        {
            Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
            Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9
        };

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
            comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.Initialize(offset, stripScale);

            BuildStrip();
        }

        public void Update()
        {
            // Если ничего не выбрано — нечего менять
            if (currentStripItemView == null)
                return;

            if (Keyboard.current == null)
                return;

            // 1) Проверяем буквы
            string pressed = GetPressedKeyName(LetterKeys);
            if (pressed == null)
            {
                // 2) Проверяем numpad
                pressed = GetPressedKeyName(NumpadKeys);
            }

            // Если в этом кадре ничего из нужного не нажали — выходим
            if (pressed == null)
                return;

            // Берём KeyData выбранного элемента и меняем KeyName
            KeyData keyData = FindKey(currentStripItemView.Id);
            keyData.KeyName = pressed;

            // Сразу обновим текст на выбранном объекте, без пересборки стрипа
            TMP_Text txt = currentStripItemView.GetComponent<TMP_Text>();
            if (txt != null)
                txt.text = pressed;
        }

        // Возвращает имя первой нажатой клавиши из списка (или null)
        private string GetPressedKeyName(Key[] keys)
        {
            foreach (var k in keys)
            {
                var key = Keyboard.current[k];
                if (key != null && key.wasPressedThisFrame)
                    return key.keyCode.ToString();
            }
            return null;
        }

        public void BuildStrip()
        {
            ClearStrip();
            comboBuilder.BuildComboStrip(recordedKeys);
        }

        public KeyData FindKey(int id)
        {
            return recordedKeys.GetEditorStripItem(id);
        }

        // удалить элемент из данных
        public void Delete(int id)
        {
            recordedKeys.Delete(id);
        }

        public void UpdateTimeByX(int id, float x)
        {
            KeyData keyData = FindKey(id);
            keyData.Time = (x - offset.x) / stripScale;
            BuildStrip();
        }

        public void Select(StripItemView stripItemView)
        {
            if (currentStripItemView != null)
            {
                currentStripItemView.Unselect();
            }
            currentStripItemView = stripItemView;
        }

        private void ClearStrip()
        {
            EditorComboBuilder comboBuilder = GetComponent<EditorComboBuilder>();
            comboBuilder.ClearContent();
        }
    }
}
