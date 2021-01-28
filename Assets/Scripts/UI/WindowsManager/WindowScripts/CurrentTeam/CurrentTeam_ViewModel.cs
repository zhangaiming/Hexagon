using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTeam_ViewModel : ViewModel
{
    TeamManager TeamManager;

    public BindableProperty<Color> TeamColor = new BindableProperty<Color>();
    public BindableProperty<string> TeamName = new BindableProperty<string>();

    public CurrentTeam_ViewModel()
    {
        GameEvent.OnTeamBeginRound += TeamChanged;
        TeamManager = GameObject.Find("GameManager").GetComponent<TeamManager>();
    }

    void TeamChanged(Controller controller)
    {
        TeamColor.Value = TeamManager.TeamColor[controller.TeamID];
        TeamName.Value = TeamManager.GetTeamName(controller.TeamID);
    }
}
