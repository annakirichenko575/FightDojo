using FightDojo.Data;
using UnityEngine;
using TMPro;

namespace FightDojo
{
    public class ComboStripFactory : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private float stripScale = 1f;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;

        private Vector2 offset;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            RecordData recordData = jsonLoader.Load();

            BuildComboStrip(recordData);
        }

        private void BuildComboStrip(RecordData recordData)
        {
            offset = Vector2.zero;
            foreach (RecordedEvent recordedEvent in recordData.recorded_events_v2)
            {
                BuildStripItem(recordedEvent, contentParent);
            }
        }

        private void BuildStripItem(RecordedEvent recordedEvent, Transform parent)
        {
            SpawnKeyText(recordedEvent, parent);
            SpawnTabImage(recordedEvent, parent);
        }

        private void SpawnTabImage(RecordedEvent recordedEvent, Transform parent)
        {
            // 2. TabImage (интервал)
            GameObject tabGO = Instantiate(tabImagePrefab, parent);
            RectTransform rect = tabGO.GetComponent<RectTransform>();
            rect.SetSiblingIndex(0);
            rect.anchoredPosition = offset;

            float widthPx = Mathf.Max(1f, recordedEvent.delay_ms * stripScale); // защита от 0
            rect.sizeDelta = new Vector2(widthPx, rect.sizeDelta.y);
            offset += Vector2.right * widthPx;
        }

        private void SpawnKeyText(RecordedEvent recordedEvent, Transform parent)
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
            keyText.text = recordedEvent.key_name_display;

            // Опционально: цвет по press/release
            TMP_Text text = keyGO.GetComponent<TMP_Text>();
            if (text != null)
            {
                
                text.color = recordedEvent.action_canonical == "press"
                    ? new Color(0.0f, 0.0f, 0.7f, 1f)
                    : new Color(0.0f, 0.0f, 0.7f, 0.6f);
            }
        }
    }
}
