using FightDojo.AudioService;
using Infrastructure.AssetManagement;
using Services;
using FightDojo.Data;
using FightDojo.UI;
using FightDojo.UI.Focus;
using FightDojo.UI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace FightDojo
{
    public class EditorComboStrip : MonoBehaviour
    {
        private readonly float minScale = 200f;
        private readonly float maxScale = 1600f;
        
        [SerializeField] private Vector2 leftOffset;
        [SerializeField] private float stripScale = 1000f;
        [SerializeField] private float scaleFactor = 200f;
        [SerializeField] private Carriage carriage;
        [SerializeField] private RectTransform contentParent;
        [SerializeField] private RectTransform inputContentParent;
        [SerializeField] private Timeline timeline;
        [SerializeField] private FocusPanel focusPanel;
        [SerializeField] private InputComboUnderWindowChecker inputComboUnderWindowChecker;
        [SerializeField] private CarriageScroller carriageScroller;
        
        private IRecordedKeysService recordedKeys;
        private EditorComboStripBuilder comboStripBuilder;
        private InputComboBuilder inputComboStripBuilder;
        private StripItemView currentStripItemView = null;
        private KeyInputReader keyInputReader = new KeyInputReader();
        private KeyTextSpawner keyTextSpawner;
        private bool isInitialized = false;

        private void Start()
        {
            Open();
        }

        public void Open()
        {
            Initialize();
            inputComboStripBuilder.ClearContent();
            BuildStrip();
        }

        private void Initialize()
        {
            if (isInitialized)
                return;
            
            recordedKeys = AllServices.Container.Single<IRecordedKeysService>();
            carriage.Initialize(contentParent);
            IAssetProvider assetProvider = AllServices.Container.Single<IAssetProvider>();
            IAudioMasterService audioMaster = AllServices.Container.Single<IAudioMasterService>();
            ICurrentComboInfoService currentComboInfo = AllServices.Container.Single<ICurrentComboInfoService>();

            keyTextSpawner = new KeyTextSpawner(stripScale, leftOffset, assetProvider);

            StripWidthSync stripWidthSync = new StripWidthSync();
            comboStripBuilder = new EditorComboStripBuilder(leftOffset, stripScale, contentParent, 
                carriage.transform, keyTextSpawner, stripWidthSync, timeline);
            
            inputComboStripBuilder = GetComponent<InputComboBuilder>();
            inputComboStripBuilder.Initialize(leftOffset, stripScale, inputContentParent, 
                carriage, keyTextSpawner, stripWidthSync, recordedKeys, audioMaster, 
                inputComboUnderWindowChecker, currentComboInfo, carriageScroller);
            
            timeline.Initialize(assetProvider, contentParent, (int)leftOffset.x);
            
            isInitialized = true;
        }

        public void Update()
        {
            if (inputComboStripBuilder.IsRecording 
                || focusPanel.IsFocused == false 
                || inputComboUnderWindowChecker.IsOpened
                || contentParent.gameObject.activeInHierarchy == false)
                return;
            
            DeleteKey();
            if (currentStripItemView == null)
            {
                AddKey();
            }
            else
            {
                UpdateKey();
            }
        }

        public void MoveKeysToLeftBorder()
        {
            
        }

        private void AddKey()
        {
            if (Keyboard.current == null || currentStripItemView != null)
                return;

            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData == null || keyData.Action != KeyData.IsPressedAction)
                return;

            float time = keyTextSpawner.GetTimeByPosition(carriage.Rect.anchoredPosition.x);
            keyData.Time = time;
            recordedKeys.Add(keyData); //insert correct id
            if (recordedKeys.Count == 1)
            {
                recordedKeys.RecalculateTime();
                Open();
                StripItemView stripItemView = contentParent.GetComponentInChildren<StripItemView>();
                SelectNewStripItem(stripItemView);
                carriage.SetPosition(keyTextSpawner.GetTimeOffset(0f).x);
                carriageScroller.Scroll();
            }
            else
            {
                StripItemView stripItemView = comboStripBuilder.BuildStripItem(keyData);
                comboStripBuilder.ResizeContent(recordedKeys.GetKeys());
                SelectNewStripItem(stripItemView);
            }
        }

        private void DeleteKey()
        {
            // Если ничего не выбрано — нечего менять
            if (currentStripItemView == null)
                return;

            if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)// вынести в Input модуль
            {
                recordedKeys.Delete(currentStripItemView.Id);
                Destroy(currentStripItemView.gameObject);
                currentStripItemView = null;
                if (recordedKeys.GetMinTime() > 0f)
                {
                    recordedKeys.RecalculateTime();
                    Open();
                }
            }
        }

        private void UpdateKey() 
        {
            if (Keyboard.current == null || currentStripItemView == null)
                return;

            KeyData inputKeyData = keyInputReader.CheckKeys();

            if (inputKeyData == null || inputKeyData.Action != KeyData.IsPressedAction)
                return;

            // Берём KeyData выбранного элемента и меняем KeyName
            recordedKeys.UpdateKeyName(currentStripItemView.Id, inputKeyData.KeyName);

            // Сразу обновим текст на выбранном объекте, без пересборки стрипа
            currentStripItemView.UpdateKey(inputKeyData.KeyName);
        }

        public void BuildStrip()
        {
            comboStripBuilder.ClearContent();
            comboStripBuilder.BuildComboStrip(recordedKeys.GetKeys());
        }

        public KeyData FindKey(int id) => 
            recordedKeys.GetEditorStripItem(id);

        // удалить элемент из данных
        public void Delete(int id)
        {
        }

        public void UpdateTimeByX(int id, float x)
        {
            recordedKeys.UpdateKeyTime(id, (x - leftOffset.x) / stripScale);
            BuildStrip();
        }

        public void MoveCarriage(PointerEventData eventData)
        {
            SetCarriagePosition(eventData);
            float time = keyTextSpawner.GetTimeByPosition(carriage.Rect.anchoredPosition.x);
            Debug.Log("!!!" + time + " " + (time*stripScale));
            SelectNewStripItem(null);
        }

        public void ChangeScale(float sign)
        {
            float oldScale = stripScale;
            stripScale += sign * scaleFactor;
            stripScale = Mathf.Clamp(stripScale, minScale, maxScale);
            float carriageTime = keyTextSpawner.GetTimeByPosition(carriage.Rect.anchoredPosition.x);
            keyTextSpawner.ChangeScale(stripScale);
            carriage.SetPosition(keyTextSpawner.GetTimeOffset(carriageTime).x);
            comboStripBuilder.ChangeScale(stripScale);
            inputComboStripBuilder.ChangeScale(stripScale);
        }
    
        public void SelectKey(StripItemView stripItemView, PointerEventData eventData)
        {
            SetCarriagePosition(eventData);
            SelectNewStripItem(stripItemView);
        }

        private void SetCarriagePosition(PointerEventData eventData) =>
            carriage.SetPosition(eventData);

        private void SelectNewStripItem(StripItemView stripItemView)
        {
            if (currentStripItemView != null)
            {
                currentStripItemView.Unselect();
            }
            currentStripItemView = stripItemView;
            if (currentStripItemView != null)
            {
                currentStripItemView.Select();
            }
        }

    }
}
