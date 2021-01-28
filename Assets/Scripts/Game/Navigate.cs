using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;

class RoutePoint
{
    public Vector3Int position;
    private float f = 0, g = 0, h = 0;
    public float F { get => f;
        set => f = value;
    }
    public float G { get => g;
        set { g = value; f = g + h; } }
    public float H { get => h;
        set { h = value; f = g + h; } }
    public RoutePoint parent;
    //public RoutePoint parent { get { return (RoutePoint)_parent; } set { _parent = value; } }

    public RoutePoint(Vector3Int pos, Vector3Int destination, Tilemap map, RoutePoint _parent = null)
    {
        position = pos;
        G = g;
        H = (map.CellToWorld(destination) - map.CellToWorld(pos)).magnitude;
        //H = (destination - pos).magnitude;
        SetParent(_parent);
    }

    public void SetParent(RoutePoint _parent)
    {
        if (_parent == null)
        {
            parent = _parent;
            G = 0;
            return;
        }
        parent = _parent;
        G = _parent.G + 1;
    }
}

public enum Direction
{
    UpRight = 0,
    UpLeft = 1,
    Right = 2,
    Left = 3,
    DownRight = 4,
    DownLeft = 5,
    Unknown = 6
}

public class Navigate : MonoBehaviour
{
    public Tilemap Map;
    public MapManager mapManager;

    public List<Vector3> GetRoute(Vector3Int origin, Vector3Int destination)
    {
        bool found = false;
        List<RoutePoint> open = new List<RoutePoint>();
        List<RoutePoint> close = new List<RoutePoint>();
        RoutePoint originPoint = new RoutePoint(origin, destination, Map);
        open.Add(originPoint);

        RoutePoint DesPoint = null;

        while (!found)
        {
            //找到F最小的路径点
            RoutePoint current = null;
            foreach(RoutePoint r in open)
            {
                if (r == null)
                {
                    open.Remove(r);
                    continue;
                }
                if(current == null || r.F < current.F)
                {
                    current = r;
                }
            }
            if(current == null)
            {
                Debug.Log("Didn't find any available route. [current == null]");
                return null;
            }
            open.Remove(current);
            close.Add(current);

            List<Vector3Int> AdjacentTile = GetAdjacentWalkableTile(current.position);
            foreach(Vector3Int v in AdjacentTile)
            {
                bool inClose = false;
                foreach(RoutePoint r in close)
                {
                    if (r.position == v)
                    {
                        inClose = true;
                        break;
                    }
                }
                if (inClose) continue;
                bool existed = false;
                foreach(RoutePoint r in open)
                {
                    if(r.position == v)
                    {
                        existed = true;
                        if (r.G > current.G + 1)
                        {
                            r.SetParent(current);
                        }
                        break;
                    }
                }
                if (!existed)
                {
                    RoutePoint routePoint = new RoutePoint(v, destination, Map, current);
                    open.Add(routePoint);
                    if(v == destination)
                    {
                        DesPoint = routePoint;
                        found = true;
                        break;
                    }
                }
            }
        }
        if(DesPoint == null)
        {
            Debug.Log("Didn't find any available route. [DesPoint == null]");
            return null;
        }
        else
        {
            List<Vector3> route = new List<Vector3>();
            RoutePoint cur = DesPoint;

            while(cur != null)
            {
                route.Add(Map.CellToWorld(cur.position));
                cur = cur.parent;
            }
            route.Reverse();
            return route;
        }
    }

    public List<Vector3Int> GetAdjacentWalkableTile(Vector3Int tile)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        int[,] dir = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        for(int i = 0; i < 4; i++)
        {
            int nx = tile.x + dir[i,0];
            int ny = tile.y + dir[i,1];
            if (mapManager.IsWalkable(nx, ny)) res.Add(new Vector3Int(nx, ny, 0));
        }
        if(tile.y % 2 == 0)
        {
            if (mapManager.IsWalkable(tile.x - 1, tile.y + 1)) res.Add(new Vector3Int(tile.x - 1, tile.y + 1, 0));
            if (mapManager.IsWalkable(tile.x - 1, tile.y - 1)) res.Add(new Vector3Int(tile.x - 1, tile.y - 1, 0));
        }
        else
        {
            if (mapManager.IsWalkable(tile.x + 1, tile.y + 1)) res.Add(new Vector3Int(tile.x + 1, tile.y + 1, 0));
            if (mapManager.IsWalkable(tile.x + 1, tile.y - 1)) res.Add(new Vector3Int(tile.x + 1, tile.y - 1, 0));
        }
        /*foreach(Vector3Int v in res)
        {
            Vector3 a = Map.CellToWorld(tile);
            a = new Vector3(a.x, a.y, -10);
            Vector3 b = Map.CellToWorld(v);
            b = new Vector3(b.x, b.y, -10);
            //Debug.DrawLine(a, b, Color.blue, 5);
        }*/
        return res;
    }

    public static List<Vector3Int> GetAdjacentTile(Vector3Int tile)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        int[,] dir = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        for (int i = 0; i < 4; i++)
        {
            int nx = tile.x + dir[i, 0];
            int ny = tile.y + dir[i, 1];
            res.Add(new Vector3Int(nx, ny, 0));
        }
        if (tile.y % 2 == 0)
        {
            res.Add(new Vector3Int(tile.x - 1, tile.y + 1, 0));
            res.Add(new Vector3Int(tile.x - 1, tile.y - 1, 0));
        }
        else
        {
            res.Add(new Vector3Int(tile.x + 1, tile.y + 1, 0));
            res.Add(new Vector3Int(tile.x + 1, tile.y - 1, 0));
        }
        return res;
    }

    public List<Vector3Int> GetWalkableTileInRange(Vector3Int origin, int range)
    {
        List<Vector3Int> closelist = new List<Vector3Int>();
        Queue<KeyValuePair<Vector3Int, int>> openlist = new Queue<KeyValuePair<Vector3Int, int>>();

        openlist.Enqueue(new KeyValuePair<Vector3Int, int>(origin, 0));

        while (openlist.Count > 0)
        {
            KeyValuePair<Vector3Int, int> cur = openlist.Dequeue();
            closelist.Add(cur.Key);
            if(cur.Value < range)
            {
                List<Vector3Int> adj = GetAdjacentWalkableTile(cur.Key);
                foreach(var v in adj)
                {
                    if(!closelist.Contains(v))
                        openlist.Enqueue(new KeyValuePair<Vector3Int, int>(v, cur.Value + 1));
                }
            }
        }
        closelist.RemoveAt(0);
        return closelist;
    }

    public static List<Vector3Int> GetTileInRange(Vector3Int origin, int range)
    {
        List<Vector3Int> closelist = new List<Vector3Int>();
        Queue<KeyValuePair<Vector3Int, int>> openlist = new Queue<KeyValuePair<Vector3Int, int>>();

        openlist.Enqueue(new KeyValuePair<Vector3Int, int>(origin, 0));

        while (openlist.Count > 0)
        {
            KeyValuePair<Vector3Int, int> cur = openlist.Dequeue();
            closelist.Add(cur.Key);
            if (cur.Value < range)
            {
                List<Vector3Int> adj = GetAdjacentTile(cur.Key);
                foreach (var v in adj)
                {
                    if (!closelist.Contains(v))
                        openlist.Enqueue(new KeyValuePair<Vector3Int, int>(v, cur.Value + 1));
                }
            }
        }
        closelist.RemoveAt(0);
        return closelist;
    }

    public static int GetDistanceIngnorePlot(Vector3Int a, Vector3Int b)
    {
        Vector3Int ap = TwoToThree(a);
        Vector3Int bp = TwoToThree(b);
        return (Math.Abs(ap.x - bp.x) + Math.Abs(ap.y - bp.y) + Math.Abs(ap.z - bp.z)) / 2;
    }

    public int GetDistance(Vector3Int a, Vector3Int b)
    {
        //List<KeyValuePair<Vector3Int, int>> closelist = new List<KeyValuePair<Vector3Int, int>>();
        List<Vector3Int> closelist = new List<Vector3Int>();
        Queue<KeyValuePair<Vector3Int, int>> openlist = new Queue<KeyValuePair<Vector3Int, int>>();

        openlist.Enqueue(new KeyValuePair<Vector3Int, int>(a, 0));
        while (openlist.Count > 0)
        {
            KeyValuePair<Vector3Int, int>  cur = openlist.Dequeue();
            closelist.Add(cur.Key);
            List<Vector3Int> adj = GetAdjacentWalkableTile(cur.Key);
            foreach (var v in adj)
            {
                if(b == v)
                {
                    return cur.Value + 1;
                }
                if (!closelist.Contains(v))
                    openlist.Enqueue(new KeyValuePair<Vector3Int, int>(v, cur.Value + 1));
            }
        }
        return 0;
    }

    /*public List<UnitBase> GetUnitInRange(Vector3Int origin, int range)
    {
        List<UnitBase> res = new List<UnitBase>();
        List<Vector3Int> plots = GetTileInRange(origin, range);
        foreach(var p in plots)
        {
            if (mapManager.IsOccupied(p))
            {
                res.Add(mapManager.GetUnitAt(p.x, p.y));
            }
        }
        return res;
    }*/

    static Vector3Int TwoToThree(Vector3Int pos)
    {
        int x = pos.x - (int)(pos.y / 2);
        int y = pos.y;
        int z = -x - y;
        return new Vector3Int(x, y, z);
    }

    /// <summary>
    /// 获得对于origin来说target所在的方向
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Direction GetDirection(Vector3 origin, Vector3 target)
    {
        Vector3 vec = (target - origin).normalized;

        float h = Vector3.Dot(vec, new Vector3(1, 0, 0));
        float v = Vector3.Dot(vec, new Vector3(0, 1, 0));

        if (h > 0 && v > 0)
            return Direction.UpRight;
        if (h < 0 && v > 0)
            return Direction.UpLeft;
        if (h == 1)
            return Direction.Right;
        if (h == -1)
            return Direction.Left;
        if (h > 0 && v < 0)
            return Direction.DownRight;
        if(h<0 && v < 0)
            return Direction.DownLeft;

        return Direction.Unknown;
    }
}
