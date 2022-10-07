using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    [SerializeField] GameObject spritePreFab;
    
    private TerrainData _data;

    public TerrainData GetData()
    { return _data; } 

    public void SetTerrain(TerrainData newData)
    {
        _data = newData;
        GetComponent<SpriteRenderer>().sprite = newData.mainTile;
    }

    public void SetSprites(TileSprite[] sprites)
    {
        // destroy existing children
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // create new children sprites
        int orderIndex = 0;
        foreach (TileSprite s in sprites)
        {
            SpriteRenderer cornerObj = Instantiate(spritePreFab, transform).GetComponent<SpriteRenderer>();
            cornerObj.sprite = s.terrain.corners[s.cornerIndex];
            cornerObj.sortingLayerName = "TerrainTransitions";
            cornerObj.sortingOrder = orderIndex;
            orderIndex++;
            SpriteRenderer sideObj = Instantiate(spritePreFab, transform).GetComponent<SpriteRenderer>();
            sideObj.sprite = s.terrain.sides[s.sideIndex];
            sideObj.sortingLayerName = "TerrainTransitions";
            sideObj.sortingOrder = orderIndex;
            orderIndex++;
        }
    }
}
