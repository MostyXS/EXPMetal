using UnityEngine;

namespace MostyProUI.Utils
{
    public static class RectEditor
    {

        public static void SetWidth(this RectTransform rect, float width)
        {
            rect.anchoredPosition = new Vector2(width, rect.anchoredPosition.y);
        }
        public static void SetHeight(this RectTransform rect, float height)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, height);
        }

        public static void SetAnchorAndSize(this RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 size)
        {
            rect.SetAnchor(anchorMin, anchorMax);
            rect.SetSize(size);
        }
        public static void SetMinAnchor(this RectTransform rect, float xMin, float yMin)
        {
            rect.anchorMin = new Vector2(xMin.Anchored(), yMin.Anchored());
        }
        public static void SetMaxAnchor(this RectTransform rect, float xMax, float yMax)
        {
            rect.anchorMax = new Vector2(xMax.Anchored(), yMax.Anchored());
        }
        public static void SetAnchor(this RectTransform rect, Vector2 anchorMin, Vector2 anchorMax)
        {
            rect.anchorMin = anchorMin.Anchored();
            rect.anchorMax = anchorMax.Anchored();
        }
        public static void SetAnchor(this RectTransform rect, Vector2 anchorBoth)
        {
            rect.anchorMin = anchorBoth;
            rect.anchorMax = anchorBoth;
        }
        public static void SetSize(this RectTransform rect, Vector2 size)
        {
            rect.sizeDelta = size;
        }
        public static void ResetSize(this RectTransform rect)
        {
            rect.sizeDelta = new Vector2(0, 0);
        }
        public static void ResetAnchor(this RectTransform rect)
        {
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 0);
        }
        public static float Anchored(this float value)
        {
            return Mathf.Clamp(value, 0, 1f);
        }
        public static Vector2 Anchored(this Vector2 vector)
        {
            return new Vector2(vector.x.Anchored(), vector.y.Anchored());
        }

    }
}
