using FightDojo.Data;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FightDojo
{
    public class TestJson : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;

        private void Start()
        {
            JsonLoader jsonLoader = new JsonLoader();
            RecordData recordData = jsonLoader.Load();

            BuildComboStrip(recordData);
        }

        private void BuildComboStrip(RecordData recordData)
        {
            foreach (var e in recordData.recorded_events_v2)
            {
                // 1. KeyText
                GameObject keyGO = Instantiate(keyTextPrefab, contentParent);
                TMP_Text keyText = keyGO.GetComponent<TMP_Text>();
                keyText.text = e.key_name_display;

                // 2. TabImage (интервал)
                GameObject tabGO = Instantiate(tabImagePrefab, contentParent);
                RectTransform tabRect = tabGO.GetComponent<RectTransform>();

                float widthPx = Mathf.Max(1f, e.delay_ms); // защита от 0
                tabRect.sizeDelta = new Vector2(widthPx, tabRect.sizeDelta.y);

                // Опционально: цвет по press/release
                Image img = tabGO.GetComponent<Image>();
                if (img != null)
                {
                    img.color = e.action_canonical == "press"
                        ? new Color(0.7f, 0.7f, 0.7f, 1f)
                        : new Color(0.5f, 0.5f, 0.5f, 0.5f);
                }
            }
        }
    }
}
