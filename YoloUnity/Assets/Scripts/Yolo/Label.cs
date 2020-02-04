using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

namespace Yolo
{
    public class Label : MonoBehaviour
    {
        Image frame;
        RectTransform frameRect;

        Text text;
        RectTransform textRect;
        Vector2 textOffset;
        float textHeight;

        private const int AdditionalTextWidth = 200;

        public void OnUpdate(Size size, Color color, YoloItem item)
        {
            gameObject.SetActive(true);

            frame.color = color;
            text.color = Color.white;
            
            RectInt r = item.Rect;
            frameRect.offsetMin = new Vector2(
                r.x * size.Factor, (size.Image.y - r.height - r.y) * size.Factor);
            frameRect.offsetMax = new Vector2(
                (r.x - (size.Image.x - r.width)) * size.Factor, -r.y * size.Factor);

            text.text = item.Depth.ToString("F0") + "m " + item.Type + " " + Mathf.Round(item.Confidence * 100) + "%";
            textRect.anchoredPosition = new Vector2(
                (r.width * size.Factor) / 2 + textOffset.x + AdditionalTextWidth / 2, textOffset.y);
            textRect.sizeDelta = new Vector2(r.width * size.Factor + AdditionalTextWidth, textHeight);
        }

        public void OnUpdate()
        {
            gameObject.SetActive(false);
        }

        void Awake()
        {
            frame = transform.GetComponent<Image>();
            frameRect = transform.GetComponent<RectTransform>();

            text = transform.GetComponentInChildren<Text>();
            //text.horizontalOverflow = HorizontalWrapMode.Overflow;
            textRect = text.GetComponent<RectTransform>();
            textOffset = new Vector2(textRect.anchoredPosition.x - textRect.sizeDelta.x / 2, textRect.anchoredPosition.y);
            textHeight = textRect.sizeDelta.y;
        }
    }
}
