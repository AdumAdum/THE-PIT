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

        backgroundImage.color = sliderData.backgroundColor;
        fillImage.color = sliderData.fillColor;
    }

    private void Start()
    {
        // Unity doesn't like when you do this in OnValidate() so I moved it here.
        slider.wholeNumbers = true;
        slider.maxValue = sliderData.max;
    }

    public void SetValue(int value)
    {
        this.slider.value = value;
    }

    public void SetMaxValue(int value)
    {
        this.slider.maxValue = value;
    }

    public void SetMinValue(int value)
    {
        this.slider.minValue = value;
    }
}
