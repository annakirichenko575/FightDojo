﻿using FightDojo.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FightDojo
{
    public class InputComboBuilder : MonoBehaviour, IStripWidth
    {
        private readonly float rightBorderOffsetX = 100f;

        [SerializeField] private float tolerance = 0.05f;
        
        private Transform contentParent;
        private Vector2 leftOffset;
        private float stripScale;
        private bool isRecording = false;
        private float maxTime = 0f;
        private KeyInputReader keyInputReader = new KeyInputReader();
        private KeyTextSpawner keyTextSpawner;
        private Carriage carriage;
        private RectTransform rectContentParent;
        private float minWidthX;
        private StripWidthSync stripWidthSync;
        private IRecordedKeysService recordedKeys;

        public bool IsRecording => isRecording;

        public void Initialize(Vector2 offset, float stripScale,
            Transform contentParent, Carriage carriage, KeyTextSpawner keyTextSpawner, StripWidthSync stripWidthSync,
            IRecordedKeysService recordedKeys)
        {
            this.carriage = carriage;
            this.stripScale = stripScale;
            this.leftOffset = offset;
            this.contentParent = contentParent;
            this.rectContentParent = contentParent.GetComponent<RectTransform>();
            this.keyTextSpawner = keyTextSpawner;
            this.stripWidthSync = stripWidthSync;
            this.stripWidthSync.InitializeStrip(this);
            this.recordedKeys = recordedKeys;
        }

        private void Update()
        {
            RecordUpdate();
            CarriageUpdate();
        }

        public void UpdateContentWidth()
        {
            float widthX = stripWidthSync.GetMaxWidth();
            if (Mathf.Approximately(widthX, rectContentParent.sizeDelta.x))
                return;
            
            rectContentParent.sizeDelta = new Vector2(
                widthX,
                rectContentParent.sizeDelta.y
            );
        }

        public float GetCurrentWidth() => 
            maxTime * stripScale + leftOffset.x + rightBorderOffsetX;
        
        private void CarriageUpdate()
        {
            if (IsRecording)
            {
                float timeLeft = keyInputReader.GetTimeLeft();
                carriage.SetCarriagePosition(keyTextSpawner.GetTimeOffset(timeLeft));
            }
        }

        private void RecordUpdate()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                if (IsRecording == true)
                    StopRecording();
                else
                    StartRecording();
            }

            if (IsRecording == true)
                InputRead();
        }

        // Чтение ввода и построение полоски
        private void InputRead()
        {
            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData == null || keyData.Action != KeyData.IsPressedAction)
                return;
            
            GameObject keyGO = BuildStripItem(keyData, contentParent);
            StripItemView stripItemView = keyGO.GetComponent<StripItemView>();
            if (false == recordedKeys.FindApproximately(keyData.KeyName, keyData.Time, tolerance))
            {
                stripItemView.SetErrorColor();
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
            SyncContentWidth();

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

        private GameObject BuildStripItem(KeyData keyData, Transform parent)
        {
            // обновляем maxTime
            if (maxTime < keyData.Time)
                maxTime = keyData.Time;

            GameObject keyGO = 
                keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, 
                    keyData.Time, keyData.KeyName, parent, true);

            // выставляем ширину Content после добавления
            SyncContentWidth();
            return keyGO;
        }

        private void SyncContentWidth()
        {
            stripWidthSync.SyncWidth();
        }

    }
}
