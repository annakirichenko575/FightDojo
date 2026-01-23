
using UnityEngine;
using TMPro;
using FightDojo.Data.AutoKeyboard;
using FightDojo.Data;

namespace FightDojo
{
    public class EditorComboBuilder : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;

        private Vector2 offset;
        private float stripScale;

        public void Initialize(Vector2 offset, float stripScale)
        {
            this.stripScale = stripScale;
            this.offset = offset;
        }

        public void BuildComboStrip(RecordedKeys recordedKeys)
        {
            foreach (KeyData keyData in recordedKeys.GetKeys())
            {
                BuildStripItem(keyData, contentParent);
            }
        }

        
        // Полная очистка Content
        public void ClearContent()
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
