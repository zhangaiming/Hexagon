using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapEditor
{
    public class MapManager : MonoBehaviour
    {
        public GridDrawer Drawer;
        public Tilemap BackgroundTilemap;
        public Tilemap MapTilemap;
        public Tilemap SpawnerTilemap;
        public Sprite SpawnerSprite;

        private Map Map;

        public Vector2Int MapSize { 
            get 
            {
                if (Map != null)
                    return Map.GetMapSize();
                else
                    return new Vector2Int(0, 0);
            } 
        }

        private void Start()
        {
            CreateMap(20, 15, "2333");
        }

        public void CreateMap(int x, int y, string name)
        {
            MapTilemap.ClearAllTiles();
            Map = new Map(new Vector2Int(x, y), name);
            Drawer.Draw();
            Vector2Int MapSize = Map.GetMapSize();
            Vector3 CenterPos = BackgroundTilemap.GetCellCenterWorld(new Vector3Int(MapSize.x / 2, MapSize.y / 2, 0));
            Camera.main.transform.position = CenterPos + new Vector3(0, 0, Camera.main.transform.position.z);
        }

        public void LoadMap(Map map)
        {
            Map = map;
            MapTilemap.ClearAllTiles();
            Drawer.Draw();
            Vector2Int size = map.GetMapSize();
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            for(int i = 0; i < size.x; i++)
            {
                for(int j = 0; j < size.y; j++)
                {
                    tile.sprite = PlotSpritePathManager.GetSprite(map.Plots[i, j]);
                    MapTilemap.SetTile(new Vector3Int(i, j, 0), tile);
                }
            }

            foreach(Vector3Int v in map.SpawnPoints)
            {
                tile.sprite = SpawnerSprite;
                SpawnerTilemap.SetTile(v, tile);
            }

            Vector3 CenterPos = BackgroundTilemap.GetCellCenterWorld(new Vector3Int(MapSize.x / 2, MapSize.y / 2, 0));
            Camera.main.transform.position = CenterPos + new Vector3(0, 0, Camera.main.transform.position.z);
        }

        public void SetUnit(PlotType type, Vector3Int pos)
        {
            bool res = Map.SetPlot(type,pos);
            if (!res)
            {
                Debug.Log("Set unit failed.");
                return;
            }
            if (type == PlotType.None)
            {
                MapTilemap.SetTile(pos, null);
                return;
            }
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = PlotSpritePathManager.GetSprite(type);
            MapTilemap.SetTile(pos, tile);
        }

        public Map GetMap()
        {
            return Map;
        }

        public void AddSpawnPoint(Vector3Int p)
        {
            if (Map.SpawnPoints.Contains(p))
                return;
            Map.SpawnPoints.Add(p);
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = SpawnerSprite;
            SpawnerTilemap.SetTile(p, tile);
        }

        public void RemoveSpawnPoint(Vector3Int p)
        {
            if (HasSpawnPointIn(p))
            {
                Map.SpawnPoints.Remove(p);
                SpawnerTilemap.SetTile(p, null);
            }
        }

        public bool HasSpawnPointIn(Vector3Int pos)
        {
            if (Map.SpawnPoints.Contains(pos))
                return true;
            return false;
        }
    }
}