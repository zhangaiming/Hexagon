using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelectManager : MonoBehaviour
{
    public MapManager mapManager;
    public Tilemap RangeTilemap;
    public Sprite RangeTile_Sprite;

    private Tile RangeTile;

    List<Vector3Int> CurrentPos = new List<Vector3Int>();
    Action<bool, Vector3Int> CurrentEventHandler;

    private void Start()
    {
        InputEvent.OnMouseLeftClickOnTilemap += Check;
        InputEvent.OnMouseRightClickOnTilemap += (pos) => { BreakOffSelect(); };
        InputEvent.OnMouseLeftClickOnUI += BreakOffSelect;
        InputEvent.OnMouseRightClickOnUI += BreakOffSelect;
    }

    void Check(Vector3Int curPos)
    {
        if (GameState.WaitingForTileSelection)
        {
            if (CurrentPos.Contains(curPos))
            {
                GameState.WaitingForTileSelection = false;
                CurrentEventHandler?.Invoke(true, curPos);
                ClearRange();
            }
        }
    }
    /// <summary>
    /// 开始选择
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="EventHandler"></param>
    public void BeginSelect(List<Vector3Int> pos, Action<bool, Vector3Int> EventHandler)
    {
        GameState.WaitingForTileSelection = true;
        CurrentPos = pos;
        CurrentEventHandler = EventHandler;
    }

    /// <summary>
    /// 中断选择
    /// </summary>
    public void BreakOffSelect()
    {
        if (GameState.WaitingForTileSelection)
        {
            CurrentEventHandler?.Invoke(false, Vector3Int.zero);
            GameState.WaitingForTileSelection = false;
            ClearRange();
        }
    }

    public void DrawRange(Vector3Int pos, Color color)
    {
        if (RangeTilemap == null)
        {
            Debug.LogError("RangeTilemap is null");
            return;
        }

        if (RangeTile == null)
        {
            if (RangeTile_Sprite == null)
            {
                Debug.LogError("RangeTile_Sprite is null");
                return;
            }
            RangeTile = ScriptableObject.CreateInstance<Tile>();
            RangeTile.sprite = RangeTile_Sprite;
        }

        RangeTile.color = color;

        RangeTilemap.SetTile(pos, RangeTile);
    }

    public void DrawRange(List<Vector3Int> pos, Color color)
    {
        if (RangeTilemap == null)
        {
            Debug.LogError("RangeTilemap is null");
            return;
        }

        foreach (var p in pos)
        {
            DrawRange(p, color);
        }
    }

    public void ClearRange()
    {
        if (RangeTilemap == null)
        {
            Debug.LogError("RangeTilemap is null");
            return;
        }

        RangeTilemap.ClearAllTiles();
    }

}
