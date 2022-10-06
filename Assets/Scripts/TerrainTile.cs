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
        Debug.Log(newData.name);
    }

    public void SetSprites(TileSprite[] sprites)
    {
        // destroy existing children
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // create new children sprites
        foreach (TileSprite s in sprites)
        {
            GameObject sideObj = Instantiate(spritePreFab, transform);
            sideObj.GetComponent<SpriteRenderer>().sprite = s.terrain.sides[s.sideIndex];
            GameObject cornerObj = Instantiate(spritePreFab, transform);
            cornerObj.GetComponent<SpriteRenderer>().sprite = s.terrain.corners[s.cornerIndex];
        }
    }
}
