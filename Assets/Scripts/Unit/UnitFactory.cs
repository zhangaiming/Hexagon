using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class UnitFactory
{

    /// <summary>
    /// 获取一个单位的实例
    /// </summary>
    /// <param name="unitname"></param>
    /// <returns>返回获取的实例,如果单位名错误,则返回null</returns>
    public static UnitBase GetUnit(string unitname, Vector3Int _pos, int _teamid)
    {
        GameObject go = GameObject.Instantiate(Resources.Load(ResourcesPaths.UnitGameObjectPrefab)) as GameObject;
        SpriteRenderer s = go.GetComponent<SpriteRenderer>();
        s.sprite = UnitSpriteManager.GetSprite(unitname);
        UnitBase unit;
        switch (unitname)
        {
            case "莽夫":
                unit = new Boor(go, _pos, _teamid);
                break;
            case "开拓者":
                unit = new Pioneer(go, _pos, _teamid);
                break;
            case "围墙":
                unit = new Wall(go, _pos, _teamid);
                break;
            default:
                Debug.LogError("UnitFactory: unavailable unit name.");
                return null;
        }
        unit.Constructed();
        GameEvent.ActiveOnUnitConstructed(unit);
        return unit;
    }
}
