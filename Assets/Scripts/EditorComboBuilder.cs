using FightDojo.Data;
using UnityEngine;
using TMPro;
using FightDojo.Data.AutoKeyboard;

namespace FightDojo
{
    public class EditorComboBuilder : MonoBehaviour
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
            SpawnKeyText(recordedEvent.id, recordedEvent.action_canonical, recordedEvent.key_name_display, parent);
            SpawnTabImage(recordedEvent.delay_ms, parent);
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

        private void SpawnKeyText(int id, string action, string keyName, Transform parent)
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

            StripItemView stripItem = keyGO.AddComponent<StripItemView>();
            stripItem.Initialize(id);

        }
    }
}
