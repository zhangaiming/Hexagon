using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace MapEditor 
{
    public class MapEditorPlotManager : MonoBehaviour
    {
        public Tilemap BackgroundTilemap;
        public MapManager mapManager;
        public GameObject CursorGO;

        private MapEditorItem selectedItem;
        public MapEditorItem SelectedItem { 
            get => selectedItem;
            set 
            { 
                if(selectedItem != null)
                {
                    selectedItem.Selected = false;
                    selectedItem.OnPointerExit();
                }
                selectedItem = value; 
                if(selectedItem != null)
                {
                    selectedItem.Selected = true;
                    if (CursorGO != null)
                    {
                        CursorGO.GetComponent<SpriteRenderer>().sprite = selectedItem.CursorSprite;
                    }
                }
                else if(selectedItem == null)
                {
                    CursorGO.GetComponent<SpriteRenderer>().sprite = null;
                }
                    

            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!EditorState.EscMenuOpened)
            {
                Vector3Int tilepos = BackgroundTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (selectedItem != null)
                {
                    if (CheckBound(tilepos.x, tilepos.y))
                    {
                        Vector3 temp = BackgroundTilemap.CellToWorld(tilepos);
                        CursorGO.transform.position = temp;
                        CursorGO.SetActive(true);
                    }
                    else
                    {
                        CursorGO.SetActive(false);
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if(selectedItem is MapEditorPlot)
                        {
                            mapManager.SetUnit(((MapEditorPlot)selectedItem).PlotType, tilepos);
                        }
                        else if(selectedItem is MapEditorSpawnPoint)
                        {
                            mapManager.AddSpawnPoint(tilepos);
                        }
                    }
                }

                if (Input.GetMouseButton(1))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (selectedItem is MapEditorPlot)
                        {
                            mapManager.SetUnit(PlotType.None, tilepos);
                        }
                        else if (selectedItem is MapEditorSpawnPoint)
                        {
                            mapManager.RemoveSpawnPoint(tilepos);
                        }
                    }
                }
            }
            else
            {
                CursorGO.SetActive(false);
            }
        }

        bool CheckBound(int x, int y)
        {
            if (x >= 0 && x < mapManager.MapSize.x && y >= 0 && y < mapManager.MapSize.y)
            {
                return true;
            }
            return false;
        }
    }

}
