using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectCursor_ViewModel : ViewModel
{
    public readonly BindableProperty<bool> ShouldShow = new BindableProperty<bool>();
    public readonly BindableProperty<Vector3> Position = new BindableProperty<Vector3>();

    void OnUnitSelected(UnitBase unit)
    {
        if(unit != null)
        {
            ShouldShow.Value = true;
            unit.OnPositionChanged += SelectedUnitPositionChanged;
            UpdatePosition(unit.Position);
        }
    }

    void OnUnitUnselected(UnitBase unit)
    {
        ShouldShow.Value = false;
        unit.OnPositionChanged -= SelectedUnitPositionChanged;
    }

    void SelectedUnitPositionChanged(UnitBase unit, Vector3Int newpos, Vector3Int oldpos)
    {
        UpdatePosition(newpos);
    }

    void UpdatePosition(Vector3Int pos)
    {
        Position.Value = GameObject.Find("Navigator").GetComponent<Navigate>().Map.CellToWorld(pos);
    }

    public SelectCursor_ViewModel()
    {
        GameEvent.OnUnitSelected += OnUnitSelected;
        GameEvent.OnUnitUnselected += OnUnitUnselected;
    }
}
