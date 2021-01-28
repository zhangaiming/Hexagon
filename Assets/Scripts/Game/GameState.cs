using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static bool IsRunning = false;
    public static bool IsUnitMoving = false;
    public static int CurrentRound = 0;
    public static int CurrentTeam = 0;

    public static int CurrentTeamNum = 0;
    public static List<int> LostTeam = new List<int>();
    public static UnitBase SelectedUnit = null;

    public static bool WaitingForTileSelection = false;

    public static void InitGameState()
    {
        IsRunning = false;
        IsUnitMoving = false;
        CurrentRound = 0;
        CurrentTeam = 0;
        CurrentTeamNum = 0;
        LostTeam = new List<int>();
        SelectedUnit = null;
        WaitingForTileSelection = false;
    }
}
