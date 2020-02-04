using UnityEngine;
using System.Collections.Generic;

namespace Yolo
{
    [System.Serializable]
    public struct LabelColor
    {
        public string descr;
        public string color;
    }

    [System.Serializable]
    public class LabelColors
    {
        public LabelColor[] labelColors;

        Dictionary<string, Color> dict;

        public void Initialize()
        {
            dict = new Dictionary<string, Color>();
            foreach (LabelColor lc in labelColors)
            {
                Color col = Color.white;
                ColorUtility.TryParseHtmlString(lc.color, out col);
                dict.Add(lc.descr, col);
            }
        }

        public Color GetColor(string descr, float depth)
        {
            if (descr.StartsWith("aero"))
            {
                if (depth < 100)
                {
                    return Color.red;
                }
                else
                {
                    return Color.green;
                }
                //return new Color(Mathf.Max(Mathf.Min(((depth - 50) / 150) * 255, 0), 255), 0, 0);
            }
            if (!dict.ContainsKey(descr))
            {
                Debug.LogWarning("Unknown object type: " + descr);
                return Color.white;
            }
            return dict[descr];
        }

        public static LabelColors CreateFromJSON(string json)
        {
            LabelColors instance = JsonUtility.FromJson<LabelColors>(json);
            instance.Initialize();
            return instance;
        }
    }
}