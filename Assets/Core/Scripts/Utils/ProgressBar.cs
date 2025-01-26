using UnityEngine;

namespace Core.Utils
{
    // TODO: Fix the bugs
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform parentRectTransform;
        [SerializeField] private RectTransform fillRectTransform;

        [SerializeField] private float maxValue = 100f;
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float currentValue = 0f;


        public void Initialize()
        {
            if (!fillRectTransform)
                fillRectTransform = GetComponentInChildren<RectTransform>();

            if (!parentRectTransform)
                parentRectTransform = fillRectTransform.parent.GetComponent<RectTransform>();

            UpdateProgressBar();
        }
        public void SetMaxValue(float value)
        {
            maxValue = value;
            UpdateProgressBar();
        }
        public void SetMinValue(float value)
        {
            minValue = value;
            UpdateProgressBar();
        }

        public void SetProgress(float value)
        {
            currentValue = Mathf.Clamp(value, minValue, maxValue);
            UpdateProgressBar();
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            float fillWidth = (currentValue / (maxValue - minValue)) * parentRectTransform.rect.width;

            fillRectTransform.sizeDelta = new Vector2(fillWidth, fillRectTransform.sizeDelta.y);
        }
    }
}
