using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Connector : Detector
{
    /// <summary>
    /// 与其他单位连接时调用
    /// </summary>
    public event Action<UnitBase, Direction> OnConnected;
    /// <summary>
    /// 与其他单位断开连接时调用
    /// </summary>
    public event Action<UnitBase, Direction> OnDisconnected;

    private UnitBase[] ConnectedUnits = new UnitBase[6];

    public Connector(UnitBase owner) : base(owner, 1)
    {
        
    }

    protected override void UnitIntoRange(UnitBase other)
    {
        Connector connector = other.GetComponent<Connector>();
        if (connector != null)
        {
            Debug.Log("Not null");
            Tilemap mapTilemap = Owner.mapManager.MapTilemap;
            Direction dir = Navigate.GetDirection(mapTilemap.CellToWorld(Owner.Position), mapTilemap.CellToWorld(other.Position));
            if (dir != Direction.Unknown)
            {
                ConnectWith(other, dir);
            }
        }
    }

    protected override void UnitOutOfRange(UnitBase other)
    {
        Connector connector = other.GetComponent<Connector>();
        if (connector != null)
        {
            
            Tilemap mapTilemap = Owner.mapManager.MapTilemap;
            Direction dir = Navigate.GetDirection(mapTilemap.CellToWorld(Owner.Position), mapTilemap.CellToWorld(other.Position));
            if (dir != Direction.Unknown)
            {
                DisconnectWith(other, dir);
            }
        }
    }

    void ConnectWith(UnitBase other, Direction dir)
    {
        if(ConnectedUnits[(int)dir] != null)
        {
            DisconnectWith(ConnectedUnits[(int)dir], dir);
        }
        OnConnected?.Invoke(other, dir);
        ConnectedUnits[(int)dir] = other;
    }

    void DisconnectWith(UnitBase other, Direction dir)
    {
        if(ConnectedUnits[(int)dir] != null)
        {
            ConnectedUnits[(int)dir] = null;
            OnDisconnected?.Invoke(other, dir);
        }
    }

    public override void SetDetectionRange(int range)
    {
        return;
    }
}
