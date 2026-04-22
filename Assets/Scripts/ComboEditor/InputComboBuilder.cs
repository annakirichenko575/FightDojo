using System.Collections.Generic;
using FightDojo.AudioService;
using FightDojo.Data;
using FightDojo.UI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FightDojo
{
    public class InputComboBuilder : MonoBehaviour, IStripWidth
    {
        private readonly float rightBorderOffsetX = 100f;

        [SerializeField] private float tolerance = 0.05f;
        
        private Vector2 leftOffset;
        private float stripScale;
        private bool isRecording = false;
        private float maxTime = 0f;
        private KeyInputReader keyInputReader = new KeyInputReader();
        private KeyTextSpawner keyTextSpawner;
        private Carriage carriage;
        private RectTransform contentParent;
        private float minWidthX;
        private StripWidthSync stripWidthSync;
        private IRecordedKeysService recordedKeys;
        private float finishRecordTime = 0;
        List<float> keyTimes = new List<float>();
        private IAudioMasterService audioMaster;
        private CountdownInputTimer countdownTimer;
        private ComboWindow underWindowChecker;


        public bool IsRecording => isRecording;

        public void Initialize(Vector2 offset, float stripScale,
            RectTransform contentParent, Carriage carriage, KeyTextSpawner keyTextSpawner, StripWidthSync stripWidthSync,
            IRecordedKeysService recordedKeys, IAudioMasterService audioMaster, ComboWindow underWindowChecker)
        {
            this.audioMaster = audioMaster;
            this.carriage = carriage;
            this.stripScale = stripScale;
            this.leftOffset = offset;
            this.contentParent = contentParent;
            this.keyTextSpawner = keyTextSpawner;
            this.stripWidthSync = stripWidthSync;
            this.stripWidthSync.InitializeStrip(this);
            this.recordedKeys = recordedKeys;
            this.countdownTimer = new CountdownInputTimer(keyInputReader, audioMaster);
            this.underWindowChecker = underWindowChecker;
        }

        private void Update()
        {
            RecordUpdate();
            CarriageUpdate();
        }

        public void UpdateContentWidth()
        {
            float widthX = stripWidthSync.GetMaxWidth();
            if (Mathf.Approximately(widthX, contentParent.sizeDelta.x))
                return;
            
            contentParent.sizeDelta = new Vector2(
                widthX,
                contentParent.sizeDelta.y
            );
        }

        public float GetCurrentWidth() => 
            maxTime * stripScale + leftOffset.x + rightBorderOffsetX;
        
        public void ChangeScale(float scale)
        {
            stripScale = scale;
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                if (contentParent.GetChild(i)
                    .TryGetComponent(out StripItemView stripItemView))
                {
                    stripItemView.ChangeScale();
                }
            }
            SyncContentWidth();
        }
        
        // Полная очистка Content
        public void ClearContent()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(contentParent.GetChild(i).gameObject);
            }
        }
        
        public void SpeedChanged(float speed)
        {
            keyInputReader.SpeedChanged(speed);
        }
        
        public void MaxCountdownTimeChanged(float time)
        {
            countdownTimer.MaxTimeChanged(time);
        }
        
        private void CarriageUpdate()
        {
            if (IsRecording)
            {
                float timeLeft = keyInputReader.GetTimeLeft();
                carriage.SetPosition(keyTextSpawner.GetTimeOffset(timeLeft).x);
                CheckNextKey(timeLeft);
            }
        }

        private void CheckNextKey(float timeLeft)
        {
            if (keyTimes.Count == 0)
                return;
            
            float nextKeyTime = keyTimes[0];
            if (nextKeyTime < timeLeft)
            {
                Debug.Log("nextTime: " + nextKeyTime + " time left: " + timeLeft);
                keyTimes.RemoveAt(0);
                audioMaster.PlayTick();
            } 
        }

        private void RecordUpdate()
        {
            if (Keyboard.current == null)
                return;

            if (underWindowChecker.IsOpened)
            {
                if (IsRecording)
                    StopRecording();
                
                return;
            }
            
            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                if (IsRecording == true)
                    StopRecording();
                else
                    StartRecording();
            }

            if (IsRecording == true)
            {
                countdownTimer.Tick();
                if (keyInputReader.IsTimerStarted)
                {
                    InputRead();
                    float timeLeft = keyInputReader.GetTimeLeft();
                    if (finishRecordTime < timeLeft)
                    {
                        StopRecording();
                    }
                }
            }
        }

        // Чтение ввода и построение полоски
        private void InputRead()
        {
            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData == null || keyData.Action != KeyData.IsPressedAction)
                return;
            
            StripItemView stripItemView = BuildStripItem(keyData, contentParent);
            if (recordedKeys.FindApproximately(keyData.KeyName, keyData.Time, tolerance))
            {
                stripItemView.SetCorrectColor();
            }
            else
            {
                stripItemView.SetErrorColor();
            }
            
        }

        // Запуск записи: очистка UI и сброс таймера
        private void StartRecording()
        {
            isRecording = true;
            finishRecordTime = recordedKeys.GetMaxTime() + 0.5f;
            keyTimes = recordedKeys.GetKeyTimes();
            keyTimes.ForEach(time => Debug.Log(time));
            ClearContent();
            countdownTimer.Reset();

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

        private StripItemView BuildStripItem(KeyData keyData, Transform parent)
        {
            // обновляем maxTime
            if (maxTime < keyData.Time)
                maxTime = keyData.Time;

            GameObject keyGO = 
                keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, 
                    keyData.Time, keyData.KeyName, parent, true);

            // выставляем ширину Content после добавления
            SyncContentWidth();
            StripItemView stripItemView = keyGO.GetComponent<StripItemView>();
            return stripItemView;
        }

        private void SyncContentWidth()
        {
            stripWidthSync.SyncWidth();
        }

    }
}
