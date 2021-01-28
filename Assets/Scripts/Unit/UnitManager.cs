using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    public GameObject UnitPrefab;
    public Tilemap maptilemap;

    private MapManager MyMapManager;

    private void Awake()
    {
        MyMapManager = GetComponent<MapManager>();
    }

    /*/// <summary>
    /// 放置一个单位
    /// </summary>
    /// <param name="unitname"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>放置成功返回true, 失败返回false</returns>
    public bool PlaceUnit(string unitname, int x, int y, int teamid)
    {
        if (!MyMapManager.IsWalkable(x, y)) {
            Debug.LogWarning("Place unit failed. -Unwalkable location");
            return false;
        }
        if (!UnitSpriteDictionary.ContainsKey(unitname))
        {
            Debug.LogError("Place unit failed! -Unavailable unit name");
            return false;
        }
        GameObject obj = GameObject.Instantiate(UnitPrefab);
        obj.transform.position = maptilemap.CellToWorld(new Vector3Int(x, y, 0));
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        Sprite spr = GetSprite(unitname);
        renderer.sprite = spr;
        UnitBase unitbase = UnitFactory.GetUnit(unitname, new Vector3Int(x, y, 0), teamid);
        if(unitbase == null)
        {
            Debug.LogError("Place unit failed! -Unitbase is null.");
            return false;
        }
        return true;
    }*/
}