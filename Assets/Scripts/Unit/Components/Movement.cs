using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class Movement : UnitComponent
{
    public event Action<Movement> BeginMove;
    public event Action<Movement> FinishMove;
    public event Action<Movement> OnMoveablityChanged;

    private bool isMoving = false;
    public bool IsMoving { get => isMoving;
        set => GameState.IsUnitMoving = isMoving = value;
    }
    private List<Vector3> route;
    private float moveTime = 0f;
    private float moveSpeed = 0.3f;

    private bool moveable = true;
    public bool Moveable { get => moveable;
        set { moveable = value; OnMoveablityChanged?.Invoke(this); } }

    TileSelectManager TileSelectManager;

    List<Vector3Int> tiles = new List<Vector3Int>();

    public Movement(UnitBase owner) : base(owner)
    {
        BeginMove += (Movement u) => { GameState.IsUnitMoving = true; };
        FinishMove += (Movement u) => { GameState.IsUnitMoving = false; };
        owner.ReadyToBeDestroyed += (u) => { ClearMoveRangeTiles(); };
        GameEvent.OnTeamEndRound += (controller) =>
        {
            if (controller.TeamID == owner.TeamID)
            {
                Moveable = true;
            }
        };
        TileSelectManager = GameObject.Find("GameManager").GetComponent<TileSelectManager>();
    }

    public bool Move(Vector3Int destination)
    {
        if (Moveable)
        {
            if(Navigate.GetDistance(Owner.Position, destination) <= Owner.ActionTime)
            {
                route = Navigate.GetRoute(Owner.Position, destination);
                if (!IsMoving && route != null)
                {
                    BeginMove?.Invoke(this);
                    IsMoving = true;
                }
                return true;
            }
        }
        return false;
    }

    public void WaitForMoveTarget()
    {
        ShowMoveRange();
        TileSelectManager.BeginSelect(tiles, 
            (success, target) =>
            {
                if (success)
                {
                    Move(target);
                }
            });
    }

    public void ShowMoveRange()
    {
        tiles = Navigate.GetWalkableTileInRange(Owner.Position, Owner.ActionTime);
        TileSelectManager.DrawRange(tiles, new Color(0, 170, 0));
    }

    public void ClearMoveRangeTiles()
    {
        TileSelectManager.ClearRange();
    }

    public override void Update()
    {
        if (IsMoving && route.Count > 1)
        {
            if (moveTime >= moveSpeed)
            {
                moveTime = 0;
                route.RemoveAt(0);
                Owner.Position = Owner.mapManager.MapTilemap.WorldToCell(route[0]);
                Owner.ActionTime -= 1;
                if (route.Count <= 1)
                {
                    IsMoving = false;
                    FinishMove?.Invoke(this);
                }
            }
            else
            {
                Owner.gameObject.transform.position = Vector3.Lerp(route[0], route[1], moveTime / moveSpeed);
                moveTime += Time.deltaTime;
            }
        }
    }
}
