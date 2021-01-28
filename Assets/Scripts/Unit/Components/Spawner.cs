using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : UnitComponent
{
    List<string> CanSpawnList = new List<string>();

    public Spawner(UnitBase owner) : base(owner)
    {

    }

    public void Spawn(string unitname, Vector3Int pos)
    {
        if (CanSpawnList.Contains(unitname) && Owner.mapManager.IsWalkable(pos))
        {
            UnitFactory.GetUnit(unitname, pos, Owner.TeamID);
        }
    }

    public List<string> GetCanSpawnList()
    {
        List<string> res = new List<string>();
        res.AddRange(CanSpawnList);
        return res;
    }
}
