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
}
