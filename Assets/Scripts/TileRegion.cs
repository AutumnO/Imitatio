using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRegion : MonoBehaviour
{
    [SerializeField] private Sprite _unselected;
    [SerializeField] private Sprite _selected;

    public GridManager gridManager;
    private SpriteRenderer _renderer;
    // children GameObjects
    private GameObject _terrain;
    private GameObject _prop;
    private TerrainData _terrainData;
    private WorldObjectData _propData;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        // check if tile is set up correctly and get terrain and prop objects
        if (transform.childCount >= 2)
        {
            _terrain = transform.GetChild(0).gameObject;
            _prop = transform.GetChild(1).gameObject;
        }
        else
            Debug.Log("Tile lacks correct amount of children!!");
        
    }

    public void PlaceObject()
    {
        // Not getting here
        Debug.Log("Placing Object");
        if (gridManager.grabbedTerrain != null)
        {
            _terrainData = gridManager.grabbedTerrain;
            _terrain.GetComponent<SpriteRenderer>().sprite = _terrainData.modelTile;
        }
        else if (gridManager.grabbedObject != null)
        {
            _propData = gridManager.grabbedObject;
            _prop.GetComponent<SpriteRenderer>().sprite = _propData.sprite;
        }
    }

    void OnMouseEnter()
    {
        _renderer.sprite = _selected;
    }
    void OnMouseExit()
    {
        _renderer.sprite = _unselected;
    }
}
