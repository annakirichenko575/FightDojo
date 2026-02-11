
using UnityEngine;
using TMPro;
using FightDojo.Data.AutoKeyboard;
using FightDojo.Data;

namespace FightDojo
{
    public class EditorComboBuilder : MonoBehaviour
    {
        private readonly float rightBorderOffsetX = 100f;

        [Header("UI")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;
        
        

        private Vector2 offset;
        private float stripScale;
        private float maxTime;
        private Transform carriage;


        public void Initialize(Vector2 offset, float stripScale, Transform carriage)
        {
            this.stripScale = stripScale;
            this.offset = offset;
            this.carriage = carriage;
        }

        public void BuildComboStrip(RecordedKeys recordedKeys)
        {
            maxTime = 0f; //зануляем максТайм
            foreach (KeyData keyData in recordedKeys.GetKeys())
            {
                BuildStripItem(keyData, contentParent);
            }
            //задаём максимальный размер Content исходя из максТайма
            RectTransform rect = contentParent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(
                maxTime * stripScale + offset.x + rightBorderOffsetX,          
                rect.sizeDelta.y 
            );
        }

        
        // Полная очистка Content
        public void ClearContent()
        {
            carriage.SetParent(contentParent.parent);
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }
            carriage.SetParent(contentParent);
        }

        private void BuildStripItem(KeyData keyData, Transform parent)
        {
            //определяем максТайм
            if (maxTime < keyData.Time)
            {
                maxTime = keyData.Time;
            }
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
