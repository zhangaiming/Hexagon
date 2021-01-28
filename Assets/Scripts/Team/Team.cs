using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Team
{
    public string TeamName;
    public Color TeamColor;
    public TeamType TeamType;

    public Team(string name, Color color, TeamType type = TeamType.Computer)
    {
        TeamName = name;
        TeamColor = color;
        TeamType = type;
    }
}
