using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightDojo.ComboEditor.Graph
{
    [RequireComponent(typeof(RawImage))]
    public class GraphBuilder : MonoBehaviour
    {
        [SerializeField] private int width = 600;
        [SerializeField] private int height = 400;
        [SerializeField] private Color lineColor = Color.green;
        [SerializeField] private int lineThick = 3;
        [SerializeField] private Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        [SerializeField] private Color axisColor = Color.white;
        [SerializeField] private int axisThick = 2;
        [SerializeField] private Color gridColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        [SerializeField] private int gridThickness = 1;
        [SerializeField] private int gridStep = 10; 
        
        private RawImage rawImage;
        private Texture2D texture;
        private bool isInitialized;

        private void Initialize()
        {
            if (isInitialized)
                return;
            
            rawImage = GetComponent<RawImage>();
            texture = new Texture2D(width, height, TextureFormat.RGBA32, false);            rawImage.texture = texture;
        }

        public void DrawGraph(List<Vector2> points)
        {
            Initialize();
            ClearGraph();
            DrawGrid();
            DrawAxis();

            if (points.Count == 1)
            {
                points[0] = new Vector2(0, points[0].y);
                points.Add(new Vector2(1, points[0].y));
            }
            
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[i + 1];

                DrawLine(
                    Mathf.RoundToInt(p1.x * (width - 1)),
                    Mathf.RoundToInt(p1.y * (height - 1)),
                    Mathf.RoundToInt(p2.x * (width - 1)),
                    Mathf.RoundToInt(p2.y * (height - 1))
                );
            }

            texture.Apply();
        }

        private void ClearGraph()
        {
            Color32[] pixels = new Color32[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = backgroundColor;
            texture.SetPixels32(pixels);
        }
       
        private void DrawGrid()
        {
            // Вертикальные линии
            for (int x = gridStep; x < width; x += gridStep)
            for (int y = 0; y < height; y++)
                DrawThickPixel(x, y, gridColor, gridThickness);

            // Горизонтальные линии
            for (int y = gridStep; y < height; y += gridStep)
            for (int x = 0; x < width; x++)
                DrawThickPixel(x, y, gridColor, gridThickness);
        }
       
        private void DrawAxis()
        {
            // Ось X (горизонтальная, y=0)
            int zeroY = 0;
            for (int x = 0; x < width; x++)
                DrawThickPixel(x, zeroY, axisColor, axisThick);

            // Ось Y (вертикальная, x=0)
            int zeroX = 0;
            for (int y = 0; y < height; y++)
                DrawThickPixel(zeroX, y, axisColor, axisThick);
        } 
        
        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            int dx = Mathf.Abs(x2 - x1);
            int dy = Mathf.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawThickPixel(x1, y1, lineColor, lineThick);

                if (x1 == x2 && y1 == y2) 
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }
        
        private void DrawThickPixel(int x, int y, Color color, int thick)
        {
            for (int ty = -thick; ty <= thick; ty++)
            {
                for (int tx = -thick; tx <= thick; tx++)
                {
                    int px = x + tx;
                    int py = y + ty;
                    if (px >= 0 && px < width && py >= 0 && py < height)
                        texture.SetPixel(px, py, color);
                }
            }
        }
    }
    
}
