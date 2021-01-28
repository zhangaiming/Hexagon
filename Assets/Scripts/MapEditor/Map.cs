using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Map
{
    public PlotType[,] Plots;
    public List<Vector3Int> SpawnPoints;
    public string Name;


    public PlotType this[int x, int y]
    {
        get
        {
            if (InBound(x, y))
            {
                return Plots[x, y];
            }
            return PlotType.None;
        }
    }

    public Map(Vector2Int mapSize, string name)
    {
        Plots = new PlotType[mapSize.x, mapSize.y];
        Name = name;
        SpawnPoints = new List<Vector3Int>();
        //AISpawnPoints = new List<Vector3Int>();
    }

    /// <summary>
    /// 获取地图的大小
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetMapSize()
    {
        if (Plots == null) return new Vector2Int(0, 0);
        return new Vector2Int(Plots.GetLength(0), Plots.GetLength(1));
    }
    /// <summary>
    /// 重新设置地图大小,比原来更大则会以PlotType.None作填充,比原来小则会进行裁剪
    /// </summary>
    /// <param name="size"></param>
    public void SetMapSize(Vector2Int size)
    {
        PlotType[,] temp = new PlotType[size.x, size.y];
        Vector2Int curSize = GetMapSize();
        for(int i = 0; i < Math.Min(curSize.x, size.x); i++)
        {
            for(int j = 0;j<Math.Min(curSize.y, size.y); j++)
            {
                temp[i, j] = Plots[i, j];
            }
        }
        Plots = temp;
    }
    /// <summary>
    /// 获取地图代码
    /// </summary>
    /// <returns></returns>
    public string GetCode()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("map::");
        sb.Append(Name).Append("::");
        //sb.Append(Plots.GetLength(0)).Append("|").Append(Plots.GetLength(1)).Append("::");
        sb.Append(VecToStr(new Vector3Int(Plots.GetLength(0), Plots.GetLength(1), 0))).Append("::");
        foreach (var vec in SpawnPoints)
        {
            sb.Append("[]").Append(VecToStr(vec));
        }
        sb.Append("::");
        /*foreach (var vec in AISpawnPoints)
        {
            sb.Append("[]").Append(VecToStr(vec));
        }
        sb.Append("::");*/
        int sizex = Plots.GetLength(0), sizey = Plots.GetLength(1);
        for(int i = 0; i < sizex; i++)
        {
            for(int j = 0; j < sizey; j++)
            {
                switch (Plots[i, j])
                {
                    case PlotType.None:
                        sb.Append(0);
                        break;
                    case PlotType.Flat:
                        sb.Append(1);
                        break;
                    case PlotType.Void:
                        sb.Append(2);
                        break;
                }
            }
        }
        sb.Append("::map");
        return sb.ToString();
    }
    /// <summary>
    /// 通过地图代码创建地图
    /// </summary>
    /// <param name="code">地图代码</param>
    /// <param name="map">创建出的地图</param>
    /// <returns>成功返回true,否则返回false</returns>
    public static bool CreateMapByCode(string code, out Map map)
    {
        map = null;
        string name;
        Vector3Int size;
        string[] module = code.Split(new string[] { "::" }, System.StringSplitOptions.None);

        if(module.Length != 6)
        {
            Debug.LogWarning("The length of module is wrong.");
            return false;
        }

        if(module[0] != "map" && module[module.Length - 1] != "map")
        {
            Debug.LogWarning("Head or tail is wrong.");
            return false;
        }
        name = module[1];
        if(StrToVec(module[2], out size))
        {
            map = new Map(new Vector2Int(size.x, size.y), name);
        }
        else
        {
            Debug.LogWarning("Size wrong");
            return false;
        }

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int temp = int.Parse(module[4][i * size.y + j].ToString());
                PlotType type;
                switch (temp)
                {
                    case 0:
                        type = PlotType.None;
                        break;
                    case 1:
                        type = PlotType.Flat;
                        break;
                    case 2:
                        type = PlotType.Void;
                        break;
                    default:
                        type = PlotType.None;
                        break;
                }
                map.Plots[i, j] = type;
            }
        }

        string[] playerSpawnPoints = module[3].Split(new string[] { "[]" }, System.StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log(playerSpawnPoints.Length);
        foreach (var s in playerSpawnPoints)
        {

            Vector3Int temp;
            if (StrToVec(s, out temp))
            {
                if (map.IsPlotWalkable(temp.x, temp.y))
                {
                    map.SpawnPoints.Add(temp);
                }
            }

        }
        
        return true;
    }
    /// <summary>
    /// 设置地块
    /// </summary>
    /// <param name="type"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>成功返回true,失败返回false</returns>
    public bool SetPlot(PlotType type, int x, int y)
    {
        if (InBound(x, y))
        {
            Plots[x, y] = type;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置地块
    /// </summary>
    /// <param name="type"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>成功返回true,失败返回false</returns>
    public bool SetPlot(PlotType type, Vector3Int pos)
    {
        return SetPlot(type, pos.x, pos.y);
    }
    /// <summary>
    /// 检查坐标处是否在界内
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool InBound(int x, int y)
    {
        Vector2Int MapSize = GetMapSize();
        if (Plots != null && x < MapSize.x && x >= 0 && y < MapSize.y && y >= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 检查坐标处是否在界内
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool InBound(Vector3Int pos)
    {
        return InBound(pos.x, pos.y);
    }
    /// <summary>
    /// 检查目标地块是否可行走
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsPlotWalkable(int x, int y)
    {
        if (!InBound(x, y))
        {
            return false;
        }
        PlotType type = Plots[x, y];
        switch (type)
        {
            case PlotType.Flat:
                return true;
            default:
                return false;
        }
    }

    static string VecToStr(Vector3Int vector)
    {
        return vector.x + "|" + vector.y + "|" + vector.z;
    }

    static bool StrToVec(string str, out Vector3Int res)
    {
        //Debug.Log(str);
        res = new Vector3Int();
        string[] s = str.Split('|');
        if (s.Length != 3)
        {
            return false;
        }
        int x, y, z;
        if(!int.TryParse(s[0],out x))
        {
            Debug.LogWarning("Can not parse x(" + s[0] + ") to integer");
            return false;
        }
        if(!int.TryParse(s[1],out y))
        {
            Debug.LogWarning("Can not parse y(" + s[1] + ") to integer");
            return false;
        }
        if(!int.TryParse(s[2],out z))
        {
            Debug.LogWarning("Can not parse z(" + s[2] + ") to integer");
            return false;
        }
        res = new Vector3Int(x, y, z);
        return true;
    }
}
