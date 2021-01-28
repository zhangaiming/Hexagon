using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pioneer : UnitBase
{
    public Pioneer(GameObject _gameObject, Vector3Int pos, int id) : base(_gameObject, "开拓者", pos, id, 15, 3)
    {
        UnitComponents.Add(new Movement(this));
        UnitComponents.Add(new Attacker(this) { AttackRange_max = 3, AttackPower = 5, AttackRange_min = 2 }) ;
    }
}
