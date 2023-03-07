using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TerrainDetails
{
    public string name;
    public float movementModifier;
}

[Serializable]
public struct WorldObjectDetails
{
    public string name;
    public Vector2 rectSize;
    public Rect[] tilesOutsideFootprint;
}

[CreateAssetMenu(fileName = "New Object List", menuName = "Assets/ObjectLists")]
public class ObjectList : ScriptableObject
{
    public TerrainData[] terrainTiles;
    public WorldObjectData[] mapDecor;
    public WorldObjectData[] buildings;
    public WorldObjectData[] other;

    public Texture2D tileSheet;
    public TerrainDetails[] terrainDetails;
    public WorldObjectDetails[] propDetails;
    public WorldObjectDetails[] buildingDetails;
    public WorldObjectDetails[] otherDetails;

    public WorldObjectDetails[] GetDetails(BasicAssetType type)
    {
        switch (type)
        {
            case BasicAssetType.Prop:
                return propDetails;
            case BasicAssetType.Building:
                return buildingDetails;
            case BasicAssetType.MiscWorld:
                return otherDetails;
            default:
                return null;
        }
    }    

    public void ResetListWithSize(BasicAssetType type, int listSize)
    {
        switch (type)
        {
            case BasicAssetType.Prop:
                mapDecor = new WorldObjectData[listSize];
                break;
            case BasicAssetType.Building:
                buildings = new WorldObjectData[listSize];
                break;
            case BasicAssetType.MiscWorld:
                other = new WorldObjectData[listSize];
                break;
            default:
                Debug.LogWarning("Warning: (ObjectList(ResetListWithSize)) was not given " +
                    "the right BasicAssetType for this list type.");
                break;
        }
    }

    public void SetListItem(BasicAssetType type, WorldObjectData asset, int index)
    {
        try
        {
            switch (type)
            {
                case BasicAssetType.Prop:
                    mapDecor[index] = asset;
                    break;
                case BasicAssetType.Building:
                    buildings[index] = asset;
                    break;
                case BasicAssetType.MiscWorld:
                    other[index] = asset;
                    break;
                default:
                    Debug.LogWarning("Warning: (AvatarCosmeticsList(SetListItem)) was not given " +
                        "the right BasicAssetType for this list type.");
                    break;
            }
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("Attempted to set an index in the AvatarCosmeticsList that doesn't exist. " +
                "Did you call ResetListWithSize before attempting to set new values?");
            throw;
        }
    }
}


