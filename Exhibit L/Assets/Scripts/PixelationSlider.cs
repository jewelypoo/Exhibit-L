using UnityEngine;
using UnityEngine.UI;

public class PixelationSlider : MonoBehaviour
{
    public Slider slider;
    public PixelationController pixelManager;

    void Start()
    {
        if (slider != null && pixelManager != null)
        {
            slider.minValue = 1;
            slider.maxValue = 10;
            slider.wholeNumbers = true;
            slider.value = pixelManager.pixelationLevel;

            slider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    void OnSliderChanged(float value)
    {
        if (pixelManager != null)
        {
            pixelManager.SetPixelationLevel(Mathf.RoundToInt(value));
        }
    }
}