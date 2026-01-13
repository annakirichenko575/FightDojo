using FightDojo.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace FightDojo
{
    public class InputComboBuilder : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private float stripScale = 1f;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;

        private Vector2 offset = Vector2.zero;
        private bool isRecording = false;

        //Счетчик id для букв
        private int nextItemId = 0;

        private KeyInputReader keyInputReader = new KeyInputReader();

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {

                if (isRecording == true)
                {
                    StopRecording();
                }
                else
                {
                    StartRecording();
                }
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

        // Запуск записи: очистка UI и сброс offset
        private void StartRecording()
        {
            isRecording = true;

            ClearContent();
            offset = Vector2.zero;

            //Сброс, чтобы новая запись начиналась с 0
            nextItemId = 0;

            //сброс таймера ввода
            keyInputReader.Reset();

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
                Destroy(contentParent.GetChild(i).gameObject);
            }
        }

        private void BuildStripItem(KeyData keyData, Transform parent)
        {
            SpawnKeyText(keyData.Action, keyData.KeyName, parent);
            SpawnTabImage(keyData.Delta, parent);
        }

        private void SpawnTabImage(float delta, Transform parent)
        {
            GameObject tabGO = Instantiate(tabImagePrefab, parent);
            RectTransform rect = tabGO.GetComponent<RectTransform>();
            rect.SetSiblingIndex(0);
            rect.anchoredPosition = offset;

            float widthPx = Mathf.Max(1f, delta * stripScale);
            rect.sizeDelta = new Vector2(widthPx, rect.sizeDelta.y);
            offset += Vector2.right * widthPx;
        }

        private void SpawnKeyText(string action, string keyName, Transform parent)
        {
            RectTransform prefabRect = keyTextPrefab.GetComponent<RectTransform>();
            float halfWidth = prefabRect.sizeDelta.x / 2f;
            offset -= Vector2.right * halfWidth;

            GameObject keyGO = Instantiate(keyTextPrefab, offset, Quaternion.identity, parent);
            RectTransform rect = keyGO.GetComponent<RectTransform>();
            rect.anchoredPosition = offset;
            offset += Vector2.right * halfWidth;

            TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
            keyText.text = keyName;

            //Повесить StripItemView и дать ему id
            StripItemView view = keyGO.GetComponent<StripItemView>();
            if (view == null)
                view = keyGO.AddComponent<StripItemView>();

            view.Initialize(nextItemId);
            nextItemId++;

            TMP_Text text = keyGO.GetComponent<TMP_Text>();
            if (text != null)
            {
                text.color = action == Constants.Press
                    ? new Color(0.0f, 0.0f, 0.7f, 1f)
                    : new Color(0.0f, 0.0f, 0.7f, 0.6f);
            }
        }
    }
}
