using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainData : ScriptableObject
{
    // name can be set due to class inheritance
    public Sprite mainTile; // the tile that represents the tile set
    public Sprite[] sides;
    public Sprite[] corners;
    public float movementModifier;
    public int dominanceIndex;

    public void Init(TerrainDetails details, int dominanceIndex, Sprite[] sprites)
    {
        name = details.name;
        movementModifier = details.movementModifier;
        this.dominanceIndex = dominanceIndex;
        if (sprites.Length == 32)
        {
            sides = new Sprite[16];
            corners = new Sprite[16];
            mainTile = sprites[0];
            sides[0] = sprites[16];
            for (int i = 1; i < 16; i++)
                sides[i] = sprites[i];
            for (int i = 16; i < 32; i++)
                corners[i - 16] = sprites[i];
        }
        else
            Debug.LogError("Recieved incorrect # of sprites for terrain data");
    }
}