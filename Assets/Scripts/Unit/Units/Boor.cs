using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boor : UnitBase
{
    public Boor(GameObject _obj, Vector3Int _pos, int _teamid) : base(_obj, UnitNameTranslator.GetNameByID(1), _pos, _teamid, 15, 2)
    {
        UnitComponents.Add(new Movement(this));
    }
}
