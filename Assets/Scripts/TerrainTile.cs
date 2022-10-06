using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainSprites
{
    middle,
    bottom, left, top, right,
    bottomLeft, topLeft, topRight, bottomRight
}

public class TerrainTile : MonoBehaviour
{
    public TerrainData data;

    public void SetSprite(Sprite sprite, TerrainSprites section)
    {
        if (section == TerrainSprites.middle)
            GetComponent<SpriteRenderer>().sprite = sprite;
        else
            transform.Find(section.ToString()).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
