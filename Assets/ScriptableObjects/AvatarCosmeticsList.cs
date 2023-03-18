using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AvatarCosmeticsList", menuName = "Assets/AvatarCosmeticsList")]
public class AvatarCosmeticsList : ScriptableObject
{
    public BasicAsset[] eyebrows;
    public BasicAsset[] eyes;
    public BasicAsset[] mouths;
    public BasicAsset[] hairs;
    public BasicAsset[] facialMarks;
    public BasicAsset[] headDecor;
    public BasicAsset[] bodies;
    public BasicAsset[] shirts;
    public BasicAsset[] outfits;
    public BasicAsset[] pants;
    public BasicAsset[] socks;
    public BasicAsset[] shoes;

    public void ResetListWithSize(BasicAssetType type, int listSize)
    {
        GetListFromType(type) = new BasicAsset[listSize];
    }    

    public void SetListItem(BasicAssetType type, BasicAsset asset, int index)
    {
        try
        {
            GetListFromType(type)[index] = asset;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("Attempted to set an index in the AvatarCosmeticsList that doesn't exist. " +
                "Did you call ResetListWithSize before attempting to set new values?");
            throw;
        }
    }

    public BasicAsset[] GetList(BasicAssetType type)
    {
        return GetListFromType(type);
    }

    public ref BasicAsset[] GetListFromType(BasicAssetType type)
    {
        switch (type)
        {
            case BasicAssetType.Eyebrow:
                return ref eyebrows;
            case BasicAssetType.Eyes:
                return ref eyes;
            case BasicAssetType.Mouth:
                return ref mouths;
            case BasicAssetType.Hair:
                return ref hairs;
            case BasicAssetType.FacialMark:
                return ref facialMarks;
            case BasicAssetType.HeadDecor:
                return ref headDecor;
            case BasicAssetType.Body:
                return ref bodies;
            case BasicAssetType.Shirt:
                return ref shirts;
            case BasicAssetType.Outfit:
                return ref outfits;
            case BasicAssetType.Pants:
                return ref pants;
            case BasicAssetType.Socks:
                return ref socks;
            case BasicAssetType.Shoes:
                return ref shoes;
            default:
                Debug.LogWarning("Warning: (AvatarCosmeticsList(GetListFromType)) was not given " +
                    "the right BasicAssetType for this list type.");
                return ref shoes;
        }
    }
}
