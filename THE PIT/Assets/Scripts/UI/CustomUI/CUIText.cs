using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CUIText : CustomUIComponent
{
    public CUITextSO textData;
    public Style style;

    private TextMeshProUGUI textMeshProUGUI;

    public override void Setup()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public override void Configure()
    {
        textMeshProUGUI.color = textData.theme.GetTextColor(style);
        textMeshProUGUI.font = textData.font;
        textMeshProUGUI.fontSize = textData.size;
    }

    public void SetText(string text)
    {
        textMeshProUGUI.text = text;
    }
}
