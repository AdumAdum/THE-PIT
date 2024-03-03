using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/ViewSO", fileName ="ViewSO")]
public class CUIViewSO : ScriptableObject
{
    public CUIThemeSO theme;
    public RectOffset padding;
    public float spacing;
}
