using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/TextSO", fileName ="Text")]
public class CUITextSO : ScriptableObject
{
    public CUIThemeSO theme;

    public TMP_FontAsset font;
    public float size;
}
