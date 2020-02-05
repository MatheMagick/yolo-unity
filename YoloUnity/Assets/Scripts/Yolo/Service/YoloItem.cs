using UnityEngine;

namespace Yolo
{
    public class YoloItem
    {
        public string Type { get; }
        public float Confidence { get; }
        public RectInt Rect { get; }
        public float Depth { get; set; }

        public YoloItem(string type, double confidence, int x, int y, float depth, int width, int height)
        {
            Type = type;
            Confidence = (float)confidence;
            Rect = new RectInt(x, y, width, height);
            Depth = depth;
        }

        public void Expand(RectInt rect)
        {
            Rect.SetMinMax(
                Vector2Int.Min(Rect.min, rect.min), 
                Vector2Int.Max(Rect.max, rect.max)
            );
        }

        public override string ToString() => $"YoloItem Type:{Type} Conf:{Confidence} Rect:{Rect}";
    }
}