using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Terrain Tile", menuName = "Assets/TerrainTile")]
public class TerrainData : ScriptableObject
{
    public Sprite modelTile; // the tile that represents the tile set
    public Sprite[] tileForms; // all possible forms of a tile set
    public float _movementModifier;
}
