using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static event Action<UnitBase> OnUnitMove;
    public static event Action<UnitBase> OnUnitSetOnMap;
    public static event Action<Controller> OnTeamBeginRound;
    public static event Action<Controller> OnTeamEndRound;
    public static event Action<UnitBase> OnUnitConstructed;
    public static event Action<UnitBase> OnUnitSelected;
    public static event Action<UnitBase> OnUnitUnselected;

    public static void ActiveOnUnitMove(UnitBase unit)
    {
        OnUnitMove?.Invoke(unit);
    }
    public static void ActiveOnUnitSetOnMap(UnitBase unit)
    {
        OnUnitSetOnMap?.Invoke(unit);
    }
    public static void ActiveOnTeamBeginRound(Controller controller)
    {
        OnTeamBeginRound?.Invoke(controller);
    }
    public static void ActiveOnTeamEndRound(Controller controller)
    {
        OnTeamEndRound?.Invoke(controller);
    }
    public static void ActiveOnUnitConstructed(UnitBase unit)
    {
        OnUnitConstructed?.Invoke(unit);
    }
    public static void ActiveOnUnitSelected(UnitBase unit)
    {
        OnUnitSelected?.Invoke(unit);
    }
    public static void ActiveOnUnitUnselected(UnitBase unit)
    {
        OnUnitUnselected?.Invoke(unit);
    }

    public static void Clear()
    {
        OnUnitMove = null;
        OnUnitSetOnMap = null;
        OnTeamBeginRound = null;
        OnTeamEndRound = null;
        OnUnitConstructed = null;
        OnUnitSelected = null;
        OnUnitUnselected = null;
    }
}
