using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CursorControl : MonoBehaviour
{
    public Tilemap MapTilemap;
    
    public bool CanSelect = false;
    private Controller currentController;
    public Controller CurrentController
    {
        get => currentController;
        set
        {
            if(value != currentController)
            {
                currentController = value;
            }
        }
    }

    MapManager mapManager;
    

    private void Start()
    {
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
    }

    void Update()
    {
        if (!GameState.IsRunning) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameState.IsUnitMoving && CanSelect && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3Int temp = MapTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //Debug.Log(Navigate.GetDistanceIngnorePlot(temp, new Vector3Int(0, 0, 0)));
                if (MapTilemap.HasTile(temp))
                {
                    if (mapManager.IsOccupied(temp))
                    {
                        UnitBase unit = mapManager.GetUnitAt(temp.x, temp.y);
                        if(unit.TeamID == CurrentController.TeamID)
                            SelectUnit(unit);
                    }
                    else
                    {
                        //UnitFactory.GetUnit("围墙", temp, TeamID);
                        UnselectUnit();
                    }
                }
                else
                {
                    UnselectUnit();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Movement movement = CurrentController.SelectedUnit.GetComponent<Movement>();
            if (CanSelect && movement != null)
            {
                Vector3Int temp = MapTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                //unitControl.TryMoveSelectedUnitTo(temp);
                movement.Move(temp);
            }
        }
    }

    void SelectUnit(UnitBase unit)
    {
        CurrentController.SelectedUnit = unit;
    }

    void UnselectUnit()
    {
        CurrentController.SelectedUnit = null;
    }

    /*void CheckState(Controller controller)
    {
        if (mapManager == null) return;
        if(CurrentController.SelectedUnit != null && MapTilemap.WorldToCell(TargetPoint) == CurrentController.SelectedUnit.Position)
        {
            Cursor_GameObject.SetActive(false);
        }
        else
        {
            Cursor_GameObject.SetActive(true);
        }

        Vector3Int cellpos = MapTilemap.WorldToCell(TargetPoint);

        if (mapManager.IsWalkable(cellpos) || mapManager.GetUnitAt(cellpos.x, cellpos.y)?.TeamID == CurrentController.TeamID)
        {
            Cursor_GameObject.GetComponent<SpriteRenderer>().sprite = Walkable_Sprite;
        }
        else
        {
            Cursor_GameObject.GetComponent<SpriteRenderer>().sprite = Unwalkable_Sprite;
        }
    }*/

    
}
