using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRound_ViewModel : ViewModel
{
    CursorControl CursorControl;

    public readonly BindableProperty<bool> ShouldShow = new BindableProperty<bool>();

    public readonly BindableProperty<bool> ShouldReact = new BindableProperty<bool>();

    public EndRound_ViewModel()
    {
        GameEvent.OnTeamBeginRound += OnBeginRound;
        CursorControl = Camera.main.GetComponent<CursorControl>();
    }

    void OnBeginRound(Controller controller)
    {
        if(controller == CursorControl.CurrentController)
        {
            ShouldShow.Value = true;
        }
    }
}
