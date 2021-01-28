using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : UnitComponent
{
    public event Action<UnitBase> OnUnitIntoRange;
    public event Action<UnitBase> OnUnitOutOfRange;

    private int detectionRange;
    public int DetectionRange => detectionRange;

    protected List<UnitBase> UnitInRange = new List<UnitBase>();

    public Detector(UnitBase owner, int range) : base(owner)
    {
        detectionRange = range;
        GameEvent.OnUnitConstructed += Check;
        owner.OnConstructed += (u) => { Detect(); };
        owner.OnPositionChanged += (u,p1,p2)=> { Detect(); };
    }

    void Detect()
    {
        List<Vector3Int> tiles = Navigate.GetTileInRange(Owner.Position, detectionRange);
        List<UnitBase> OutOfRangeUnits = new List<UnitBase>();
        OutOfRangeUnits.AddRange(UnitInRange);
        foreach(var t in tiles)
        {
            UnitBase unit = Owner.mapManager.GetUnitAt(t.x, t.y);
            if (unit != null && !UnitInRange.Contains(unit))
            {
                if (OutOfRangeUnits.Contains(unit))
                {
                    OutOfRangeUnits.Remove(unit);
                }
                UnitInRange.Add(unit);
                OnUnitIntoRange?.Invoke(unit);
                UnitIntoRange(unit);
            }
        }
        foreach(var unit in OutOfRangeUnits)
        {
            if (UnitInRange.Contains(unit))
            {
                UnitInRange.Remove(unit);
                OnUnitOutOfRange?.Invoke(unit);
                UnitOutOfRange(unit);
            }
        }
    }

    void Check(UnitBase unit)
    {
        if (unit != null && unit != Owner)
        {
            int dis = Navigate.GetDistanceIngnorePlot(unit.Position, Owner.Position);
            if(dis <= detectionRange && !UnitInRange.Contains(unit))
            {
                Debug.Log(dis);
                UnitInRange.Add(unit);
                OnUnitIntoRange?.Invoke(unit);
                UnitIntoRange(unit);
            }else if(dis > detectionRange && UnitInRange.Contains(unit))
            {
                UnitInRange.Remove(unit);
                OnUnitOutOfRange?.Invoke(unit);
                UnitOutOfRange(unit);
            }
        }
    }

    protected virtual void UnitIntoRange(UnitBase other)
    {
        
    }

    protected virtual void UnitOutOfRange(UnitBase other)
    {
        
    }

    public virtual void SetDetectionRange(int range)
    {
        detectionRange = range;
    }
}
