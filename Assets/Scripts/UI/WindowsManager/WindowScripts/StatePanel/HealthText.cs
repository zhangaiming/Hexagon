using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : GUIBase<StatePanel_ViewModel>
{
    public Text text;
    public ToolTipControl ToolTip;

    int cur = 0, max = 0;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        Binder.Add<int>("UnitHealth", OnUnitHealthChanged);
        Binder.Add<int>("UnitMaxHealth", OnUnitMaxHealthChanged);
    }

    void OnUnitHealthChanged(int newValue, int oldValue)
    {
        cur = newValue;
        string t = "生命值：" + cur + "/" + max;
        text.text = t;
        ToolTip.text = t;
    }

    void OnUnitMaxHealthChanged(int newValue, int oldValue)
    {
        max = newValue;
        string t = "生命值：" + cur + "/" + max;
        text.text = t;
        ToolTip.text = t;
    }
}
