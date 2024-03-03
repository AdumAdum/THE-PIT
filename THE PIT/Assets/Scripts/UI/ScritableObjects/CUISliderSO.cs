using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CustomUI/SliderSO",fileName ="Slider")]
public class CUISliderSO : ScriptableObject
{
    public bool interactable;
    public Color backgroundColor;
    public Color fillColor;
    public float min;
    public float max;
}
