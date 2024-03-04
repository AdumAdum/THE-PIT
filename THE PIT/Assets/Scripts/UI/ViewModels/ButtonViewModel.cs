using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonViewModel : MonoBehaviour
{
    public CUIButton button;

    [Header("Event")]
    public SpecificGameEvent onClick;

    // When not testing, other things (he says models) will send data to viewmodel to invoke events
    // This is just for example purposes
    //[Header("Event Data")]
    //public int value;

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        onClick.Raise(this);
    }
}
