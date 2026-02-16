﻿using FightDojo.Data;
using Infrastructure.AssetManagement;
using Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FightDojo
{
    public class InputComboBuilder : MonoBehaviour
    {
        private readonly float rightBorderOffsetX = 100f;

        private Transform contentParent;
        private Vector2 offset;
        private float stripScale;
        private bool isRecording = false;

        private float maxTime = 0f;

        private KeyInputReader keyInputReader = new KeyInputReader();
        private KeyTextSpawner keyTextSpawner;

        public void Initialize(Vector2 offset, float stripScale, 
            Transform contentParent, Transform carriage, KeyTextSpawner keyTextSpawner)
        {
            this.stripScale = stripScale;
            this.offset = offset;
            this.contentParent = contentParent;
            this.keyTextSpawner = keyTextSpawner;
        }

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                if (isRecording == true)
                    StopRecording();
                else
                    StartRecording();
            }

            if (isRecording == true)
                InputRead();
        }

        // Чтение ввода и построение полоски
        private void InputRead()
        {
            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData != null)
            {
                BuildStripItem(keyData, contentParent);
            }
        }

        // Запуск записи: очистка UI и сброс таймера
        private void StartRecording()
        {
            isRecording = true;
            ClearContent();

            // сброс таймера ввода и id
            keyInputReader.Reset();

            // сброс maxTime для новой записи
            maxTime = 0f;

            // сразу сбросим ширину контента 
            UpdateContentWidth();

            Debug.Log("InputCombo recording started");
        }

        // Остановка записи
        private void StopRecording()
        {
            isRecording = false;
            Debug.Log("InputCombo recording stopped");
        }

        // Полная очистка Content
        private void ClearContent()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(contentParent.GetChild(i).gameObject);
            }
        }

        private void BuildStripItem(KeyData keyData, Transform parent)
        {
            // обновляем maxTime
            if (maxTime < keyData.Time)
                maxTime = keyData.Time;

            keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, keyData.Time, keyData.KeyName, parent);

            // выставляем ширину Content после добавления
            UpdateContentWidth();
        }

        private void UpdateContentWidth()
        {
            RectTransform rect = contentParent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(
                maxTime * stripScale + offset.x + rightBorderOffsetX,
                rect.sizeDelta.y
            );
        }
    }
}
