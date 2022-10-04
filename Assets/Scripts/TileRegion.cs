using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRegion : MonoBehaviour
{
    // Prefab
    [SerializeField] private Sprite _unselected;

    private SpriteRenderer _renderer;

    // Terrain and Prop Objects (children)
    private SpriteRenderer _terrain;
    private SpriteRenderer _prop;
    private TerrainData _terrainData;
    private WorldObjectData _propData;
    private TileRegion _propOrigin;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        // check if tile is set up correctly and get terrain and prop objects
        if (transform.childCount >= 2)
        {
            _terrain = GetComponentsInChildren<SpriteRenderer>()[1];
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
    public void PlaceObject(TerrainData obj, Transform loc)
    {
        _terrainData = obj;
        _terrain.sprite = _terrainData.modelTile;
        _terrain.transform.position = loc.position;
        _propOrigin = this;
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
        _renderer.sprite = _unselected;
    }
}
