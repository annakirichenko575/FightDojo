using System.Collections.ObjectModel;
using UnityEngine;
using FightDojo.Data;
using Object = UnityEngine.Object;

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
        private Timeline timeline;

        public EditorComboStripBuilder(Vector2 leftOffset, float stripScale,
            Transform contentParent, Transform carriage, KeyTextSpawner keyTextSpawner, 
            StripWidthSync stripWidthSync, Timeline timeline)
        {
            this.timeline = timeline;
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
            Transform timelineTransform = timeline.transform;
            carriage.SetParent(contentParent.parent);
            timelineTransform.SetParent(contentParent.parent);
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(contentParent.GetChild(i).gameObject);
            }
            timelineTransform.SetParent(contentParent);
            carriage.SetParent(contentParent);
        }

        public StripItemView BuildStripItem(KeyData keyData)
        {
            //определяем максТайм
            if (maxTime < keyData.Time)
                maxTime = keyData.Time;
            
            GameObject keyGO = keyTextSpawner.SpawnKeyText(keyData.Id, keyData.Action, keyData.Time, keyData.KeyName, contentParent);
            StripItemView stripItemView = keyGO.GetComponent<StripItemView>();
            return stripItemView;
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
            if (false == Mathf.Approximately(widthX, rectContentParent.sizeDelta.x))
            {
                rectContentParent.sizeDelta = new Vector2(
                    widthX,          
                    rectContentParent.sizeDelta.y 
                );
            }
            timeline.CreateTimeline(stripScale);
            timeline.transform.SetAsLastSibling();
            carriage.SetAsLastSibling();
        }

        public float GetCurrentWidth() => 
            maxTime * stripScale + leftOffset.x + rightBorderOffsetX;

        public void ChangeScale(float scale)
        {
            stripScale = scale;
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                if (contentParent.GetChild(i)
                    .TryGetComponent(out StripItemView stripItemView))
                {
                    stripItemView.ChangeScale();
                }
            }
            SyncContentWidth();
        }
    
        private void SyncContentWidth()
        {
            stripWidthSync.SyncWidth();
        }

    }
}
