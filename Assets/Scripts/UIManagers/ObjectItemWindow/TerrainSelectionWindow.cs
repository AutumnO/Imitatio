using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerrainSelectionWindow : ObjectSelectionWindow
{
    private TerrainData[] _terrainTiles;
    public override int GetItemList()
    {
        _terrainTiles = _gridManager.GetTerrainTiles();
        return _terrainTiles.Length;
    }
    public override Sprite GetWindowSprite(int index)
    {
        return _terrainTiles[index].mainTile;
    }
    public override void SetListener(EventTrigger.Entry entry, int dataIndex)
    {
        entry.callback.AddListener((func) => { _gridManager.GrabObject(_terrainTiles[dataIndex]); });
    }
}
