using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGame : GUIBase<CreateGame_ViewModel>
{
    public GameObject ColorPan_GameObject;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Binder.Add<bool>("ShouldShowColorPan", OnShouldShowColorPanChanged);
    }

    void OnShouldShowColorPanChanged(bool newValue, bool oldValue)
    {
        ColorPan_GameObject.SetActive(newValue);
    }

    public void OnColorButtonClick()
    {

    }

    public void OnAddTeamButtonClick()
    {

    }

    public void OnBeginGameButtonClick()
    {

    }

    public void OnCancelButtonClick()
    {
        gameObject.SetActive(false);
    }
}
