using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Object", menuName = "Assets/WorldObject")]
public class WorldObjectData : BasicAsset
{
    public Vector2 rectSize;
    public Rect[] tilesOutsideFootprint;
}
