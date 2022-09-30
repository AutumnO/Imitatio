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
    [SerializeField] private Sprite _testSprite;

    private Dictionary<Vector2, TileRegion> _tiles;

    // object placement
    private WorldObjectData _grabbedObject;
    private TerrainData _grabbedTerrain;

    private void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                PlaceObject(hit.collider.gameObject.GetComponent<TileRegion>());
            }
        }
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
                tileSpawn.name = $"Tile {x} {y}";

                _tiles[new Vector2(x, y)] = tileSpawn;
            }
        }
    }

    public void GrabObject(WorldObjectData data)
    {
        _grabbedObject = data;
        _grabbedTerrain = null;
    }
    public void GrabTerrain(TerrainData data)
    {
        _grabbedTerrain = data;
        _grabbedObject = null;
    }
    public void PlaceObject(TileRegion tile)
    {
        if (_grabbedObject != null)
            tile.PlaceObject(_grabbedObject);
        else if (_grabbedTerrain != null)
            tile.PlaceTerrain(_grabbedTerrain);
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
}
