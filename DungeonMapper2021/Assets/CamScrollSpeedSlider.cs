using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CamScrollSpeedSlider : MonoBehaviour {

    public Slider slider;

    //OnSliderValueChanged triggers before Awake resetting the value of camScrollSpeed. This bool ensures that it doesn't update until slider is initiated
    bool isSliderInitiated = false; 
    void Awake()
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();

            if (slider == null)
            {
                Debug.Log("No slider assigned to camScrollSlider script");
                return;
            }
        }

        slider.wholeNumbers = true;

        slider.maxValue = PreferencesData.maxScrollSpeed;
        slider.minValue = PreferencesData.minScrollSpeed;
        slider.value = StaticVariables.Preferences.camScrollSpeed;
        UpdateDisplayTextValue(StaticVariables.Preferences.camScrollSpeed);

        isSliderInitiated = true;

    }

    public void OnSliderValueChanged()
    {
        if (!isSliderInitiated)
        {
            return;
        }

        StaticVariables.Preferences.camScrollSpeed = (int)slider.value;

        UpdateDisplayTextValue(StaticVariables.Preferences.camScrollSpeed);
    }

    public Text DisplayValueText;

    void UpdateDisplayTextValue(int currentValue)
    {
        if (DisplayValueText != null)
        {
            DisplayValueText.text = currentValue.ToString();
        }
    }

}
