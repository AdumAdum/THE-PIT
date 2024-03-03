using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUISlider : CustomUIComponent
{
    public CUISliderSO sliderData;

    public Slider slider;
    public Image backgroundImage;
    public Image fillImage;

    public override void Setup() 
    { 
        
    }

    public override void Configure()
    {
        slider.interactable = sliderData.interactable;
        slider.minValue = sliderData.min;
        slider.maxValue = sliderData.max;
        backgroundImage.color = sliderData.backgroundColor;
        fillImage.color = sliderData.fillColor;
    }

    public void SetValue(float value)
    {
        this.slider.value = value;
    }
}
