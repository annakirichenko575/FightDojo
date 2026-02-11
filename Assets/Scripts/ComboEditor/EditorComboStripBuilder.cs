
using UnityEngine;
using FightDojo.Data;

namespace FightDojo
{
    public class EditorComboStripBuilder : MonoBehaviour
    {
        private readonly float rightBorderOffsetX = 100f;

        [Header("UI")]
        [SerializeField] private GameObject keyTextPrefab;
        [SerializeField] private GameObject tabImagePrefab;
        
        private Vector2 offset;
        private float stripScale;
        private float maxTime;
        private Transform carriage;
        private KeyTextSpawner keyTextSpawner;
        private Transform contentParent;

        public void Initialize(Vector2 offset, float stripScale, Transform contentParent, Transform carriage)
        {
            this.stripScale = stripScale;
            this.offset = offset;
            this.contentParent = contentParent;
            this.carriage = carriage;
            keyTextSpawner = new KeyTextSpawner(stripScale, offset, keyTextPrefab);
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
            keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, keyData.Time, keyData.KeyName, parent);
        }
    }
}
