using System;
using Infrastructure.AssetManagement;
using Services;
using FightDojo.Data;
using FightDojo.Data.AutoKeyboard;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

namespace FightDojo
{
    public class EditorComboStrip : MonoBehaviour
    {
        [SerializeField] private Vector2 offset;
        [SerializeField] private float stripScale = 2000f;
        [SerializeField] private Carriage carriage;
        [SerializeField] private Transform contentParent;
        [SerializeField] private Transform InputContentParent;


        private RecordedKeys recordedKeys;
        private EditorComboStripBuilder comboStripBuilder;
        private InputComboBuilder inputComboStripBuilder;
        private StripItemView currentStripItemView = null;
        private KeyInputReader keyInputReader = new KeyInputReader();
        private KeyTextSpawner keyTextSpawner;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            recordedKeys = RecordDataAdapter.Adapt(jsonLoader.Load());
            
            carriage.Initialize(contentParent.GetComponent<RectTransform>());
            IAssetProvider assetProvider = AllServices.Container.Single<IAssetProvider>();

            keyTextSpawner = new KeyTextSpawner(stripScale, offset, assetProvider);

            comboStripBuilder = new EditorComboStripBuilder(offset, stripScale, contentParent, 
                carriage.transform, keyTextSpawner);

            inputComboStripBuilder = GetComponent<InputComboBuilder>();
            inputComboStripBuilder.Initialize(offset, stripScale, InputContentParent, 
                carriage.transform, keyTextSpawner);
            
            BuildStrip();
        }

        public void Update()
        {
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

        private void AddKey()
        {
            if (Keyboard.current == null || currentStripItemView != null)
                return;

            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData == null || keyData.Action != KeyData.IsPressedAction)
                return;

            float time = keyTextSpawner.GetTimeByPosition(carriage.Rect.anchoredPosition.x);
            KeyData inputKeyData = new KeyData(keyData.Id, keyData.Action, time, keyData.KeyName);

            GameObject keyGO = comboStripBuilder.BuildStripItem(inputKeyData);
            SelectNewStripItem(keyGO.GetComponent<StripItemView>());
        }

        private void DeleteKey()
        {
            // Если ничего не выбрано — нечего менять
            if (currentStripItemView == null)
                return;

            if (Keyboard.current != null && Keyboard.current.deleteKey.wasPressedThisFrame)// вынести в Input модуль
            {
                Delete(currentStripItemView.Id);
                Destroy(currentStripItemView.gameObject);
                currentStripItemView = null;
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
            TMP_Text txt = currentStripItemView.GetComponent<TMP_Text>();
            if (txt != null)
                txt.text = inputKeyData.KeyName;
        }

        public void BuildStrip()
        {
            comboStripBuilder.ClearContent();
            comboStripBuilder.BuildComboStrip(recordedKeys);
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
            recordedKeys.UpdateKeyTime(id, (x - offset.x) / stripScale);
            BuildStrip();
        }

        public void MoveCarriage(PointerEventData eventData)
        {
            SetCarriagePosition(eventData);
            SelectNewStripItem(null);
        }

        public void SelectKey(StripItemView stripItemView, PointerEventData eventData)
        {
            SetCarriagePosition(eventData);
            SelectNewStripItem(stripItemView);
        }

        private void SetCarriagePosition(PointerEventData eventData) =>
            carriage.SetCarriagePosition(eventData);

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
