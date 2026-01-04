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

        private KeyInputReader keyInputReader = new KeyInputReader();

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            // ▶ START RECORDING — Space
            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                StartRecording();
                return;
            }

            // ⏹ STOP RECORDING — Left Ctrl
            if (Keyboard.current[Key.LeftCtrl].wasPressedThisFrame)
            {
                StopRecording();
                return;
            }

            // Если запись не активна — Update ничего не делает
            if (!isRecording)
                return;

            // Чтение ввода и построение полоски
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

            // 🔑 СБРОС ТАЙМЕРА ВВОДА
            keyInputReader.Reset();

            UnityEngine.Debug.Log("InputCombo recording started");
        }

        // Остановка записи
        private void StopRecording()
        {
            isRecording = false;
            UnityEngine.Debug.Log("InputCombo recording stopped");
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
