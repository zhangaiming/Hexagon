using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Cursor_ViewModel : ViewModel
{
    MapManager mapManager;
    GameManager GameManager;

    bool EnteredUI = false;

    public readonly BindableProperty<bool> Hide = new BindableProperty<bool>();
    public readonly BindableProperty<Vector3> Position = new BindableProperty<Vector3>();
    public readonly BindableProperty<bool> Walkable = new BindableProperty<bool>();

    public Cursor_ViewModel()
    {
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        GameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        InputEvent.OnCursorPositionChanged += CursorPositionChanged;
        InputEvent.OnMouseOverUIStateChanged += MouseOverUIStateChanged;
    }

    void MouseOverUIStateChanged(bool state)
    {
        EnteredUI = state;
    }

    void CursorPositionChanged(Vector3Int pos)
    {
        if (EnteredUI)
        {
            Hide.Value = true;
            return;
        }
        if(mapManager != null)
        {
            Position.Value = GameManager.StandardTilemap.CellToWorld(pos);
            if(mapManager.GetPlotTypeAt(pos) == PlotType.None)
            {
                Hide.Value = true;
            }
            else
            {
                Hide.Value = false;
            }

            if (mapManager.IsWalkable(pos))
            {
                if (Hide.Value)
                    Hide.Value = false;
                Walkable.Value = true;
            }
            else
            {
                if (mapManager.IsOccupied(pos))
                {
                    UnitBase unit = mapManager.GetUnitAt(pos.x, pos.y);
                    if (GameState.CurrentTeam == unit.TeamID)
                    {
                        if(GameState.SelectedUnit == unit)
                        {
                            Hide.Value = true;
                        }
                        else
                        {
                            if (Hide.Value)
                                Hide.Value = false;
                            Walkable.Value = true;
                        }
                    }
                    else
                    {
                        if (Hide.Value)
                            Hide.Value = false;
                        Walkable.Value = false;
                    }
                }else
                {
                    if (Hide.Value)
                        Hide.Value = false;
                    Walkable.Value = false;
                }
            }
        }
    }
}
