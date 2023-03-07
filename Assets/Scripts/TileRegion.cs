using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRegion : MonoBehaviour
{
    // Prefab
    [SerializeField] private Sprite _unselected;

    private SpriteRenderer _renderer;

    // Terrain and Prop Objects (children)
    private SpriteRenderer _prop;
    private WorldObjectData _propData;
    private TileRegion _propOrigin;

    public TerrainTile terrainTile;
    private TerrainData _defaultTerrain;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        // check if tile is set up correctly and get terrain and prop objects
        if (transform.childCount >= 2)
        {
            terrainTile = GetComponentInChildren<TerrainTile>();
            _prop = GetComponentsInChildren<SpriteRenderer>()[2];
        }
        else
            Debug.Log("Tile lacks correct amount of children!!");

        _propOrigin = this;
    }

    public void SetDefaultTerrain(TerrainData defaultTerrain)
    {
        terrainTile.SetTerrain(defaultTerrain);
        _defaultTerrain = defaultTerrain;
    }
    public void PlaceObject(WorldObjectData obj, Transform loc, TileRegion propOrigin)
    {
        _propData = obj;
        _prop.sprite = _propData.mainSprite;
        _prop.transform.position = loc.position;
        _propOrigin = propOrigin;
    }
    public void PlaceObject(TerrainData data, Transform loc, TileRegion[] surrounding5x5Grid)
    {
        terrainTile.SetTerrain(data);
        terrainTile.transform.position = loc.position;
        _propOrigin = this;
        RelationalTiles.SetTileSprites(surrounding5x5Grid, _defaultTerrain);
    }
    public bool HasObject()
    {
        if (_propData == null)
            return false;
        else
            return true;
    }
    void OnMouseEnter()
    {
        _renderer.sprite = null;
    }
    void OnMouseExit()
    {
        //_renderer.sprite = _unselected;
    }
}
