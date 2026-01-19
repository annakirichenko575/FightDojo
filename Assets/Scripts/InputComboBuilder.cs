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

        [SerializeField] private Vector2 offset;
        private bool isRecording = false;

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
            SpawnKeyText(keyData.Id, keyData.Action, keyData.Time, keyData.KeyName, parent);
            
        }

        private void SpawnKeyText(int id, string action, float time, string keyName, Transform parent)
        {
            Vector2 right = Vector2.right * (time * stripScale) + offset;

            GameObject keyGO = Instantiate(keyTextPrefab, parent);
            
            RectTransform keyRect = keyGO.GetComponent<RectTransform>();
            keyRect.anchoredPosition = new Vector2(right.x, right.y);

            TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
            keyText.text = keyName;

            TMP_Text text = keyGO.GetComponent<TMP_Text>();
            if (text != null)
            {
                text.color = action == Constants.Press
                    ? new Color(0.0f, 0.0f, 0.7f, 1f)
                    : new Color(0.0f, 0.0f, 0.7f, 0.6f);
            }
            
            StripItemView stripItem = keyGO.AddComponent<StripItemView>();
            stripItem.Initialize(id);
        }
    }
}
