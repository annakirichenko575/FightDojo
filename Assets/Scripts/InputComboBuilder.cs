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
        private KeyInputReader keyInputReader = new KeyInputReader();

        // Проверка нажатий и отпусканий клавиш каждый кадр
        private void Update()
        {
            if (Keyboard.current == null)
                return;

            KeyData keyData = keyInputReader.CheckKeys();
            if (keyData != null)
            {
                BuildStripItem(keyData, contentParent);
            }
        }

        private void BuildStripItem(KeyData keyData, Transform parent)
        {
            SpawnKeyText(keyData.Action, keyData.KeyName, parent);
            SpawnTabImage(keyData.Delta, parent);
        }

        private void SpawnTabImage(float delta, Transform parent)
        {
            // 2. TabImage (интервал)
            GameObject tabGO = Instantiate(tabImagePrefab, parent);
            RectTransform rect = tabGO.GetComponent<RectTransform>();
            rect.SetSiblingIndex(0);
            rect.anchoredPosition = offset;

            float widthPx = Mathf.Max(1f, delta * stripScale); // защита от 0
            rect.sizeDelta = new Vector2(widthPx, rect.sizeDelta.y);
            offset += Vector2.right * widthPx;
        }

        private void SpawnKeyText(string action, string keyName, Transform parent)
        {
            // 1. KeyText
            RectTransform prefabRect = keyTextPrefab.GetComponent<RectTransform>();
            float halfWidth = prefabRect.sizeDelta.x / 2f;
            offset -= Vector2.right * halfWidth;

            GameObject keyGO = Instantiate(keyTextPrefab, offset, Quaternion.identity, parent);
            RectTransform rect = keyGO.GetComponent<RectTransform>();
            rect.anchoredPosition = offset;
            offset += Vector2.right * halfWidth;

            TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
            keyText.text = keyName;

            // цвет по press/release
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
