using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapEditor
{
    public class GridDrawer : MonoBehaviour
    {
        public Tilemap background_tilemap;
        public MapManager mapManager;
        public Sprite Background_Sprite;

        public void Draw()
        {
            if(mapManager == null)
            {
                Debug.LogError("Draw grid failed. Map manager is null.");
                return;
            }
            background_tilemap.ClearAllTiles();
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Background_Sprite;
            for(int i = 0; i < mapManager.MapSize.x; i++)
            {
                for(int j = 0; j < mapManager.MapSize.y; j++)
                {
                    background_tilemap.SetTile(new Vector3Int(i, j, 0), tile);
                }
            }
        }
    }
}
