using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    // width and height of visible grid
    [SerializeField] private Transform _camera;
    [SerializeField] private int _width, _height;
    [SerializeField] private TileRegion _emptyTilePrefab;
    [SerializeField] private ObjectList _objectList;

    private Dictionary<Vector2, TileRegion> _tiles;

    // object placement
    public WorldObjectData grabbedObject;
    public TerrainData grabbedTerrain;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // create dictionary to hold spawned tiles
        _tiles = new Dictionary<Vector2, TileRegion>();

        // create "empty tile" objects
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileRegion tileSpawn = Instantiate(_emptyTilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawn.gridManager = this;
                tileSpawn.name = $"Tile {x} {y}";

                _tiles[new Vector2(x, y)] = tileSpawn;
            }
        }
    }

    public TileRegion GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out TileRegion returnedTile))
            return returnedTile;
        return null;
    }

    public TerrainData[] GetTerrainTiles()
    {
        return _objectList.terrainTiles;
    }
    public WorldObjectData[] GetWorldObjects(ObjectType type)
    {
        if (type == ObjectType.Prop)
            return _objectList.mapDecor;
        else if (type == ObjectType.Building)
            return _objectList.buildings;
        else
            return _objectList.other;
    }

    public void GrabObject(WorldObjectData data)
    {
        grabbedObject = data;
    }
    public void GrabTerrain(TerrainData data)
    {
        grabbedTerrain = data;
    }

    public void PlaceObject()
    {

    }
}
