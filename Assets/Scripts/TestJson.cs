using FightDojo.Data;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FightDojo
{
    public class ComboStripFactory : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private float stripScale = 1f;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;

        private Vector3 offset;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            RecordData recordData = jsonLoader.Load();

            BuildComboStrip(recordData);
        }

        private void BuildComboStrip(RecordData recordData)
        {
            offset = Vector3.zero;
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
            GameObject tabGO = Instantiate(tabImagePrefab, offset, Quaternion.identity, parent);
            RectTransform tabRect = tabGO.GetComponent<RectTransform>();

            float widthPx = Mathf.Max(1f, recordedEvent.delay_ms * stripScale); // защита от 0
            tabRect.sizeDelta = new Vector2(widthPx, tabRect.sizeDelta.y);
            offset += Vector3.right * widthPx;

        }

        private void SpawnKeyText(RecordedEvent recordedEvent, Transform parent)
        {
            // 1. KeyText
            RectTransform rect = keyTextPrefab.GetComponent<RectTransform>();
            float halfWidth = rect.sizeDelta.x / 2f;
            offset -= Vector3.right * halfWidth;

            GameObject keyGO = Instantiate(keyTextPrefab, offset, Quaternion.identity, parent);
            offset += Vector3.right * halfWidth;

            TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
            keyText.text = recordedEvent.key_name_display;

            // Опционально: цвет по press/release
            TMP_Text text = keyGO.GetComponent<TMP_Text>();
            if (text != null)
            {
                text.color = recordedEvent.action_canonical == "press"
                    ? new Color(0.0f, 0.7f, 0.0f, 1f)
                    : new Color(0.0f, 0.7f, 0.0f, 0.7f);
            }
        }
    }
}
