using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextViewModel : MonoBehaviour
{
    public CUIText textView;

    public void onValueChanged(Component sender, object data)
    {
        if (data is int health)
        {
            string healthString = health.ToString();
            textView.SetText(healthString);
        }
    }
}
