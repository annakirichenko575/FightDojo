
using System.Collections.ObjectModel;
using UnityEngine;
using FightDojo.Data;

namespace FightDojo
{
    public class EditorComboStripBuilder: IStripWidth
    {
        private readonly float rightBorderOffsetX = 2000f;

        private Vector2 leftOffset;
        private float stripScale;
        private float maxTime;
        private Transform carriage;
        private KeyTextSpawner keyTextSpawner;
        private StripWidthSync stripWidthSync;
        private Transform contentParent;
        private RectTransform rectContentParent;

        public EditorComboStripBuilder(Vector2 leftOffset, float stripScale,
            Transform contentParent, Transform carriage, KeyTextSpawner keyTextSpawner, StripWidthSync stripWidthSync)
        {
            this.stripScale = stripScale;
            this.leftOffset = leftOffset;
            this.contentParent = contentParent;
            this.rectContentParent = contentParent.GetComponent<RectTransform>();
            this.carriage = carriage;
            this.keyTextSpawner = keyTextSpawner;
            this.stripWidthSync = stripWidthSync;
            this.stripWidthSync.InitializeStrip(this);
        }

        public void BuildComboStrip(ReadOnlyCollection<KeyData> allKeys)
        {
            maxTime = 0f; //зануляем максТайм
            foreach (KeyData keyData in allKeys)
            {
                BuildStripItem(keyData);
            }
            //задаём максимальный размер Content исходя из максТайма
            SyncContentWidth();
        }

        // Полная очистка Content
        public void ClearContent()
        {
            carriage.SetParent(contentParent.parent);
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(contentParent.GetChild(i).gameObject);
            }
            carriage.SetParent(contentParent);
        }

        public GameObject BuildStripItem(KeyData keyData)
        {
            //определяем максТайм
            if (maxTime < keyData.Time)
                maxTime = keyData.Time;
                
            return keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, keyData.Time, keyData.KeyName, contentParent);
        }
        
        public void ResizeContent(ReadOnlyCollection<KeyData> allKeys)
        {
            maxTime = 0f; //зануляем максТайм
            foreach (KeyData keyData in allKeys)
            {
                if (maxTime < keyData.Time)
                    maxTime = keyData.Time;
            }
            SyncContentWidth();
        } 
        
        public void UpdateContentWidth()
        {
            float widthX = stripWidthSync.GetMaxWidth();
            if (Mathf.Approximately(widthX, rectContentParent.sizeDelta.x))
                return;
            
            rectContentParent.sizeDelta = new Vector2(
                widthX,          
                rectContentParent.sizeDelta.y 
            );
        }

        public float GetCurrentWidth() => 
            maxTime * stripScale + leftOffset.x + rightBorderOffsetX;

        private void SyncContentWidth()
        {
            stripWidthSync.SyncWidth();
        }

    }
}
