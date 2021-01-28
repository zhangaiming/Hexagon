using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tilemap MapTilemap;
    private Map map;

    private UnitBase[,] unitMap = new UnitBase[1, 1];

    public Tilemap RangeTilemap;

    public bool LoadMap(string code)
    {
        if(Map.CreateMapByCode(code, out map))
        {
            Vector2Int MapSize = map.GetMapSize();
            unitMap = new UnitBase[MapSize.x, MapSize.y];
            for (int i = 0; i < MapSize.x; i++)
            {
                for(int j = 0; j < MapSize.y; j++)
                {
                    DrawPlot(map[i, j], i, j);
                }
            }
            Vector3 CenterPos = MapTilemap.GetCellCenterWorld(new Vector3Int(MapSize.x / 2, MapSize.y / 2, 0));
            Camera.main.transform.position = CenterPos + new Vector3(0, 0, Camera.main.transform.position.z);
            return true;
        }
        return false;
    }

    void DrawPlot(PlotType type, int x, int y)
    {
        if (!CheckBound(x, y))
            return;
        Tile temp = ScriptableObject.CreateInstance<Tile>();
        Sprite sprite = PlotSpritePathManager.GetSprite(type);
        if (sprite == null)
        {
            //Debug.LogError("Unable to get the sprite of the type: " + type.ToString());
            return;
        }
        temp.sprite = sprite;
        MapTilemap.SetTile(new Vector3Int(x, y, 0), temp);
    }

    public void SetPlot(PlotType type, int x, int y)
    {
        if (!map.SetPlot(type, x, y))
            return;
        Tile temp = ScriptableObject.CreateInstance<Tile>();
        Sprite sprite = PlotSpritePathManager.GetSprite(type);
        if(sprite == null)
        {
            Debug.LogError("Unable to get the sprite of the type: " + type.ToString());
            return;
        }
        temp.sprite = sprite;

        MapTilemap.SetTile(new Vector3Int(x, y, 0), temp);
    }
    public void SetPlot(PlotType type, Vector3Int pos)
    {
        SetPlot(type, pos.x, pos.y);
    }
    public void SetUnit(UnitBase unit, int x, int y)
    {
        if (!CheckBound(x, y))
        {
            Debug.LogWarning("Set unit failed. Out of bounds.");
            return;
        }
        unitMap[x, y] = unit;
        GameEvent.ActiveOnUnitSetOnMap(unit);
    }
    public void SetUnit(UnitBase unit, Vector3Int pos)
    {
        SetUnit(unit, pos.x, pos.y);
    }
    public bool SetUnit(string unitname, int x, int y, int teamid)
    {
        return SetUnit(unitname, new Vector3Int(x, y, 0), teamid);
    }
    public bool SetUnit(string unitname, Vector3Int pos, int teamid)
    {
        if (!CheckBound(pos.x, pos.y))
        {
            Debug.LogWarning("Set unit failed. Out of bounds.");
            return false;
        }
        UnitBase unit= UnitFactory.GetUnit(unitname, pos, teamid);
        if(unit == null)
        {
            return false;
        }
        //unit.gameObject.transform.position = MapTilemap.CellToWorld(pos);
        unitMap[pos.x, pos.y] = unit;
        GameEvent.ActiveOnUnitSetOnMap(unit);
        return true;
    }

    public void RemoveUnitAt(int x, int y)
    {
        if (CheckBound(x, y))
        {
            unitMap[x, y] = null;
        }
    }

    public bool IsPlotWalkable(int x, int y)
    {
        return map.IsPlotWalkable(x, y);
    }
    public bool IsPlotWalkable(Vector3Int pos)
    {
        return IsPlotWalkable(pos.x, pos.y);
    }

    public bool IsWalkable(int x, int y)
    {
        if (!CheckBound(x, y))
        {
            return false;
        }
        if (IsOccupied(x, y))
        {
            return false;
        }

        return IsPlotWalkable(x, y);
    }
    public bool IsWalkable(Vector3Int pos)
    {
        return IsWalkable(pos.x, pos.y);
    }

    public bool IsOccupied(int x, int y)
    {
        if (!CheckBound(x, y))
        {
            return false;
        }
        if (unitMap[x, y] != null)
        {
            return true;
        }
        return false;
    }
    public bool IsOccupied(Vector3Int pos)
    {
        return IsOccupied(pos.x, pos.y);
    }

    public UnitBase GetUnitAt(int x, int y)
    {
        if (!CheckBound(x, y))
        {
            return null;
        }
        return unitMap[x, y];
    }

    public PlotType GetPlotTypeAt(int x, int y)
    {
        if (!map.InBound(x, y))
            return PlotType.None;
        return map[x, y];
    }
    public PlotType GetPlotTypeAt(Vector3Int pos)
    {
        return GetPlotTypeAt(pos.x, pos.y);
    }

    public List<Vector3Int> GetSpawnPoints()
    {
        if(map == null)
        {
            return new List<Vector3Int>();
        }
        return map.SpawnPoints;
    }

    bool CheckBound(int x, int y)
    {
        return map.InBound(x, y);
    }
}
