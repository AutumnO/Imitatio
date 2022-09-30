using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ObjectType
{
    Prop, Building, Other
};

[CreateAssetMenu(fileName = "New World Object", menuName = "Assets/WorldObject")]
public class WorldObjectData : ScriptableObject
{
    public Sprite sprite;
    public ObjectType type;
}
