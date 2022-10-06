using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object List", menuName = "Assets/ObjectLists")]
public class ObjectList : ScriptableObject
{
    public TerrainData[] terrainTiles;
    public WorldObjectData[] mapDecor;
    public WorldObjectData[] buildings;
    public WorldObjectData[] other;

    public Texture2D tileSheet;
    public TerrainDetails[] terrainDetails;
}

[Serializable]
public struct TerrainDetails
{
    public string name;
    public float movementModifier;
}
