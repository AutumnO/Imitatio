using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    // width and height of visible grid
    [SerializeField] private Transform _camera;
    [SerializeField] private int _width, _height;
    [SerializeField] private TileRegion _emptyTilePrefab;
    [SerializeField] private GameObject _mouseAttachment;
    [SerializeField] private ObjectList _objectList;

    private Dictionary<TileRegion, Vector2> _tiles;

    // object placement
    private GameObject _grabbedObjMouse;
    private GameObject _grabbedObjPreview;
    private WorldObjectData _grabbedProp;
    private TerrainData _grabbedTerrain;

    private Vector3 _mousePos;

    private void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        // attach sprite to the mouse when something is grabbed,
        // display preview on tiles when hovering over existing tiles
        if (_grabbedObjMouse != null && _grabbedObjPreview != null)
        {
            
            TileRegion tile = GetTileAtMousePos();
            if (tile != null) // hovering over tile, display preview
            {
                _grabbedObjMouse.SetActive(false);
                _grabbedObjPreview.SetActive(true);
                Vector2 previewPos = _tiles[tile];
                if (_grabbedProp != null)
                {
                    if (_grabbedProp.rectSize.x % 2 == 0)
                        previewPos.x -= 0.5f;
                    if (_grabbedProp.rectSize.y % 2 == 0)
                        previewPos.y += 0.5f;
                }
                _grabbedObjPreview.transform.position = previewPos;
            }
            else // not hovering over tile, display next to mouse
            {
                _grabbedObjMouse.SetActive(true);
                _grabbedObjPreview.SetActive(false);
                _mousePos = Input.mousePosition;
                _grabbedObjMouse.transform.position = new Vector2(_mousePos.x + 20, _mousePos.y - 20);
            }
        }
        // attempt to place a grabbed object when a tile is clicked
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject(GetTileAtMousePos());
        }
    }

    private TileRegion GetTileAtMousePos()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_mousePos.x, _mousePos.y), Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject.GetComponent<TileRegion>();
        }
        else
            return null;
    }

    void GenerateGrid()
    {
        // create dictionary to hold spawned tiles
        _tiles = new Dictionary<TileRegion, Vector2>();

        // create "empty tile" objects
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileRegion tileSpawn = Instantiate(_emptyTilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawn.name = $"Tile {x} {y}";

                _tiles[tileSpawn] = new Vector2(x, y);
            }
        }
    }

    public void GrabProp(WorldObjectData data)
    {
        _grabbedProp = data;
        _grabbedTerrain = null;
        GrabbedObjectHandler(data.sprite);
    }
    public void GrabTerrain(TerrainData data)
    {
        _grabbedTerrain = data;
        _grabbedProp = null;
        GrabbedObjectHandler(data.modelTile);
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
    public void PlaceObject(TileRegion tile)
    {
        if (tile != null)
        {
            if (_grabbedProp != null)
                tile.PlaceObject(_grabbedProp, _grabbedObjPreview.transform);
            else if (_grabbedTerrain != null)
                tile.PlaceTerrain(_grabbedTerrain, _grabbedObjPreview.transform);
        }
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
