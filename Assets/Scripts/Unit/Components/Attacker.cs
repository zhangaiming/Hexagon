using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : UnitComponent
{
    public event Action<Attacker> OnAttackabilityChanged;

    private int attackRange_min = 0;
    /// <summary>
    /// 最小攻击距离
    /// </summary>
    public int AttackRange_min { get => attackRange_min;
        set => attackRange_min = Mathf.Clamp(value, 0, AttackRange_max);
    }
    private int attackRange_max = 0;
    /// <summary>
    /// 最大攻击距离
    /// </summary>
    public int AttackRange_max { get => attackRange_max;
        set => attackRange_max = Mathf.Max(AttackRange_min, value);
    }
    private int attackPower;
    /// <summary>
    /// 基准攻击力
    /// </summary>
    public int AttackPower { get => attackPower;
        set => attackPower = value;
    }

    private bool canAttack = true;
    public bool CanAttack { get => canAttack;
        set { canAttack = value; OnAttackabilityChanged?.Invoke(this); } }

    TileSelectManager TileSelectManager;

    public Attacker(UnitBase owner) : base(owner)
    {
        TileSelectManager = GameObject.Find("GameManager").GetComponent<TileSelectManager>();
    }

    public void ClearRange()
    {
        TileSelectManager.ClearRange();
    }

    public void WaitForAttackTarget()
    {
        List<Vector3Int> max = Navigate.GetTileInRange(Owner.Position, AttackRange_max);
        if (AttackRange_min != 0)
        {
            List<Vector3Int> temp = new List<Vector3Int>();
            foreach (var pos in max)
            {
                if (Navigate.GetDistanceIngnorePlot(pos, Owner.Position) >= AttackRange_min)
                {
                    Debug.Log(pos + "," + Owner.Position + Navigate.GetDistanceIngnorePlot(pos, Owner.Position));
                    if (MapManager.IsPlotWalkable(pos))
                    {
                        temp.Add(pos);
                    }
                }
            }
            max = temp;
        }

        foreach (var pos in max)
        {
            if (MapManager.IsOccupied(pos))
            {
                UnitBase unit = MapManager.GetUnitAt(pos.x, pos.y);
                if (unit.TeamID != Owner.TeamID)
                {
                    TileSelectManager.DrawRange(pos, new Color(255, 69, 170));
                    continue;
                }
            }
            TileSelectManager.DrawRange(pos, new Color(0, 170, 0));
        }

        TileSelectManager.DrawRange(max, new Color(0, 170, 0));

        List<UnitBase> units = new List<UnitBase>();
        List<Vector3Int> targetVec = new List<Vector3Int>();
        foreach(var p in max)
        {
            UnitBase unit = MapManager.GetUnitAt(p.x, p.y);
            if(unit != null && unit.TeamID != Owner.TeamID)
            {
                units.Add(unit);
                targetVec.Add(p);
            }
        }

        TileSelectManager.BeginSelect(targetVec, (success, result) => 
        { 
            if (success) 
            { 
                Attack(MapManager.GetUnitAt(result.x, result.y)); 
            } 
        });
    }

    public void Attack(UnitBase target)
    {
        if(target != null)
        {
            int ran = UnityEngine.Random.Range(-3, 3);
            target.Health -= Mathf.Max(ran + AttackPower, 1);
        }
    }
}
