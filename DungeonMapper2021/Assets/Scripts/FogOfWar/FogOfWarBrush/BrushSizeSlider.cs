using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BrushSizeSlider : MonoBehaviour
{

    private void Awake()
    {
        InitializeSlider();
    }

    public Slider brushSizeSlider;
    public void InitializeSlider()
    {
        if (brushSizeSlider == null)
        {
            brushSizeSlider = gameObject.GetComponent<Slider>();
        }


        if (brushSizeSlider != null)
        {
            brushSizeSlider.minValue = PreferencesData.minBrushSize;
            brushSizeSlider.maxValue = PreferencesData.maxMaxBrushSize;
            brushSizeSlider.value = StaticVariables.Preferences.BrushSize;

            brushSizeSlider.onValueChanged.AddListener(OnSliderValueChanged);

            if (brushSizeText != null)
            {
                brushSizeText.text = StaticVariables.Preferences.BrushSize.ToString();
            }

        }
    }

    public Text brushSizeText;
    public void OnSliderValueChanged(float currentValue)
    {
        StaticVariables.Preferences.BrushSize = (int)currentValue;

        if (brushSizeText != null)
        {
            brushSizeText.text = ((int)currentValue).ToString();
        }
    }
}
