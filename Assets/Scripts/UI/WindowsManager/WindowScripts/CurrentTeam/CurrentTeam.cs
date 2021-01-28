using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTeam : GUIBase<CurrentTeam_ViewModel>
{
    public Image image;
    public ToolTipControl ToolTip;

    private void Start()
    {
        BindingContext = new CurrentTeam_ViewModel();
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Binder.Add<Color>("TeamColor", OnColorChanged);
        Binder.Add<string>("TeamName", OnNameChanged);
    }

    void OnColorChanged(Color newvalue, Color oldvalue)
    {
        image.color = newvalue;
    }

    void OnNameChanged(string newValue, string oldValue)
    {
        ToolTip.text = newValue;
    }
}