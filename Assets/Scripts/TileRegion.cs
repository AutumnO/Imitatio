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

    private TerrainTile _terrainTile;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        // check if tile is set up correctly and get terrain and prop objects
        if (transform.childCount >= 2)
        {
            _terrainTile = GetComponentInChildren<TerrainTile>();
            _prop = GetComponentsInChildren<SpriteRenderer>()[2];
        }
        else
            Debug.Log("Tile lacks correct amount of children!!");

        _propOrigin = this;
    }

    public void PlaceObject(WorldObjectData obj, Transform loc, TileRegion propOrigin)
    {
        _propData = obj;
        _prop.sprite = _propData.sprite;
        _prop.transform.position = loc.position;
        _propOrigin = propOrigin;
    }
    public void PlaceObject(TerrainData data, Transform loc)
    {
        _terrainTile.data = data;
        _terrainTile.SetSprite(data.mainTile, TerrainSprites.middle);
        _terrainTile.transform.position = loc.position; //???
        _propOrigin = this;
    }
    public bool HasObject()
    {
        if (_propData == null)
            return false;
        else
            return true;
    }
    public TerrainData GetTerrainData()
    {
        return _terrainTile.data;
    }
    void OnMouseEnter()
    {
        _renderer.sprite = null;
    }
    void OnMouseExit()
    {
        _renderer.sprite = _unselected;
    }
}
