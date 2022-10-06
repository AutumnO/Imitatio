using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private int _width, _height;   // width and height of visible grid
    [SerializeField] private ObjectList _objectList;
    // Prefabs
    [SerializeField] private TileRegion _emptyTilePrefab;
    [SerializeField] private GameObject _mouseAttachment;
    
    // Tile Dictionaries
    private Dictionary<TileRegion, Vector2> _tiles;
    private Dictionary<Vector2, TileRegion> _tilePositions;

    // Object Placement Variables
    private GameObject _grabbedObjMouse;
    private GameObject _grabbedObjPreview;
    private WorldObjectData _grabbedProp;
    private TerrainData _grabbedTerrain;

    private bool _isPlaceable;
    private Color _placeableColor;
    private Color _unplaceableColor;

    private Vector3 _mousePos;
    

    private void Start()
    {
        _isPlaceable = false;
        _placeableColor = Color.green;
        _unplaceableColor = Color.red;
        GenerateGrid();
    }

    private void Update()
    {
        if (_grabbedObjMouse != null && _grabbedObjPreview != null)
        {
            TileRegion tile = GetTileAtMousePos();
            // if mouse is over tiles, display preview on tiles
            // if not, display icon next to mouse
            if (tile != null) 
            {
                _grabbedObjMouse.SetActive(false);
                _grabbedObjPreview.SetActive(true);
                Vector2 previewPos = _tiles[tile];
                
                if (_grabbedProp != null)
                    previewPos = HandlePropPreview(tile); // is object placeable + grid snapping adjustment
                else if (_grabbedTerrain != null)
                    _isPlaceable = true;

                _grabbedObjPreview.transform.position = previewPos;
            }
            else
            {
                _grabbedObjMouse.SetActive(true);
                _grabbedObjPreview.SetActive(false);
                _mousePos = Input.mousePosition;
                _grabbedObjMouse.transform.position = new Vector2(_mousePos.x + 20, _mousePos.y - 20);
            }
        }
        
        if (Input.GetMouseButtonDown(0) && _isPlaceable == true)
        {
            PlaceObject(GetTileAtMousePos());
        }
    }

    private TileRegion GetTileAtMousePos()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_mousePos.x, _mousePos.y), Vector2.zero);
        if (hit.collider != null)
            return hit.collider.gameObject.GetComponent<TileRegion>();
        else
            return null;
    }

    void GenerateGrid()
    {
        // create dictionary to hold spawned tiles
        _tiles = new Dictionary<TileRegion, Vector2>();
        _tilePositions = new Dictionary<Vector2, TileRegion>();

        // create "empty tile" objects
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileRegion tileSpawn = Instantiate(_emptyTilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawn.name = $"Tile {x} {y}";

                _tiles[tileSpawn] = new Vector2(x, y);
                _tilePositions[new Vector2(x, y)] = tileSpawn;
            }
        }
    }

    public void GrabObject(WorldObjectData data)
    {
        _grabbedProp = data;
        _grabbedTerrain = null;
        GrabbedObjectHandler(data.sprite);
    }
    public void GrabObject(TerrainData data)
    {
        _grabbedTerrain = data;
        _grabbedProp = null;
        GrabbedObjectHandler(data.mainTile);
    }
    private void GrabbedObjectHandler(Sprite sprite)
    {
        Destroy(_grabbedObjMouse);
        _grabbedObjMouse = Instantiate(_mouseAttachment);
        _grabbedObjMouse.transform.SetParent(GameObject.Find("Canvas").transform, false);
        _grabbedObjMouse.GetComponent<Image>().sprite = sprite;
        _grabbedObjMouse.GetComponent<Image>().preserveAspect = true;

        Destroy(_grabbedObjPreview);
        _grabbedObjPreview = new GameObject();
        SpriteRenderer renderer = _grabbedObjPreview.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingLayerName = "Overlay";
        _grabbedObjPreview.SetActive(false);
    }
    private Vector2 HandlePropPreview(TileRegion tile)
    {
        List<TileRegion> affectedTiles = GetAffectedTiles(tile, _grabbedProp);
        if (affectedTiles != null)
        {
            _isPlaceable = true;
            foreach (TileRegion t in affectedTiles)
            {
                if (t.HasObject() == true)
                {
                    _grabbedObjPreview.GetComponent<SpriteRenderer>().color = _unplaceableColor;
                    _isPlaceable = false;
                    break;
                }
            }
            if (_isPlaceable)
                _grabbedObjPreview.GetComponent<SpriteRenderer>().color = _placeableColor;
        }
        else
        {
            _grabbedObjPreview.GetComponent<SpriteRenderer>().color = _unplaceableColor;
            _isPlaceable = false;
        }

        // adjust position if width or height span even # of tiles
        Vector2 previewPos = _tiles[tile];
        if (_grabbedProp.rectSize.x % 2 == 0)
            previewPos.x -= 0.5f;
        if (_grabbedProp.rectSize.y % 2 == 0)
            previewPos.y -= 0.5f;
        return previewPos;
    }
    public void PlaceObject(TileRegion originTile)
    {
        if (_grabbedProp != null && originTile != null)
        {
            List<TileRegion> affectedTiles = GetAffectedTiles(originTile, _grabbedProp);
            if (affectedTiles != null)
            {
                foreach (TileRegion tile in affectedTiles)
                {
                    tile.PlaceObject(_grabbedProp, _grabbedObjPreview.transform, originTile);
                }
            }
        }
        else if (_grabbedTerrain != null && originTile != null)
            originTile.PlaceObject(_grabbedTerrain, _grabbedObjPreview.transform);
    }
    private List<TileRegion> GetAffectedTiles(TileRegion tile, WorldObjectData obj)
    {
        // if no other tiles are affected, just return the origin tile
        if (obj.rectSize.x * obj.rectSize.y == 1)
            return new List<TileRegion>() { tile };

        List<TileRegion> affectedTiles = new List<TileRegion>();
        Vector2 center = _tiles[tile];
        Vector2 origin = new Vector2(0, 0);
        Vector2 end = new Vector2(0, 0);
        origin.x = center.x - Mathf.Floor(obj.rectSize.x / 2);
        origin.y = center.y - Mathf.Floor(obj.rectSize.y / 2);
        end.x = origin.x + obj.rectSize.x;
        end.y = origin.y + obj.rectSize.y;

        for (int i = (int)origin.x; i < end.x; i++)
        {
            for (int j = (int)origin.y; j < end.y; j++)
            {
                if (_tilePositions.TryGetValue(new Vector2(i, j), out TileRegion currTile))
                    affectedTiles.Add(currTile);
                else
                    return null;
            }
        }

        return affectedTiles;
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
