using FightDojo.Data;
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

        public bool IsRecording => isRecording;

        public void Initialize(Vector2 offset, float stripScale,
            RectTransform contentParent, Carriage carriage, KeyTextSpawner keyTextSpawner, StripWidthSync stripWidthSync,
            IRecordedKeysService recordedKeys)
        {
            this.carriage = carriage;
            this.stripScale = stripScale;
            this.leftOffset = offset;
            this.contentParent = contentParent;
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
        
        private void CarriageUpdate()
        {
            if (IsRecording)
            {
                float timeLeft = keyInputReader.GetTimeLeft();
                carriage.SetPosition(keyTextSpawner.GetTimeOffset(timeLeft).x);
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
