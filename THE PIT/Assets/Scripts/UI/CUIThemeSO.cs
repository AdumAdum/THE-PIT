using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/ThemeSO", fileName = "Theme")]
public class CUIThemeSO : ScriptableObject
{
    [Header("Primary")]
    public Color primary_bg;
    public Color primary_text;

    [Header("Secondary")]
    public Color secondary_bg;
    public Color secondary_text;

    [Header("Tertiary")]
    public Color tertiary_bg;
    public Color tertiary_text;

    [Header("Other")]
    public Color disable;

    public Color GetBackGroundColor(Style style)
    {
        switch (style)
        {
            case Style.Primary: return primary_bg;
            case Style.Secondary: return secondary_bg;
            case Style.Tertiary: return tertiary_bg;
            default: return disable;
        }
    }

    public Color GetTextColor(Style style)
    {
        switch (style)
        {
            case Style.Primary: return primary_text;
            case Style.Secondary: return secondary_text;
            case Style.Tertiary: return tertiary_text;
            default: return disable;
        }
    }
}
