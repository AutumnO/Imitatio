using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRegion : MonoBehaviour
{
    [SerializeField] private Sprite _unselected;
    [SerializeField] private Sprite _selected;

    private SpriteRenderer _renderer;
    // children GameObjects
    private SpriteRenderer _terrain;
    private SpriteRenderer _prop;
    private TerrainData _terrainData;
    private WorldObjectData _propData;

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
        
    }

    public void PlaceObject(WorldObjectData obj, Transform loc)
    {
        _propData = obj;
        _prop.sprite = _propData.sprite;
        _prop.transform.position = loc.position;
    }
    public void PlaceTerrain(TerrainData obj, Transform loc)
    {
        _terrainData = obj;
        _terrain.sprite = _terrainData.modelTile;
        _terrain.transform.position = loc.position;
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
