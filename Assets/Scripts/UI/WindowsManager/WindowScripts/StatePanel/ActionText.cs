using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionText : GUIBase<StatePanel_ViewModel>
{
    public Text text;
    public ToolTipControl ToolTip;

    int cur = 0, max = 0;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        Binder.Add<int>("UnitActionTime", OnUnitActionTimeChanged);
        Binder.Add<int>("UnitMaxActionTime", OnUnitMaxActionTimeChanged);
    }

    void OnUnitActionTimeChanged(int newValue, int oldValue)
    {
        cur = newValue;
        string t = "行动力：" + cur + "/" + max;
        text.text = t;
        ToolTip.text = t;
    }

    void OnUnitMaxActionTimeChanged(int newValue, int oldValue)
    {
        max = newValue;
        string t = "行动力：" + cur + "/" + max;
        text.text = t;
        ToolTip.text = t;
    }
}
