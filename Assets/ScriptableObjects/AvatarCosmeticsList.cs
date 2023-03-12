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
    public BasicAsset[] shirts;
    public BasicAsset[] outfits;
    public BasicAsset[] pants;
    public BasicAsset[] socks;
    public BasicAsset[] shoes;

    public void ResetListWithSize(BasicAssetType type, int listSize)
    {
        switch (type)
        {
            case BasicAssetType.Eyebrow:
                eyebrows = new BasicAsset[listSize];
                break;
            case BasicAssetType.Eyes:
                eyes = new BasicAsset[listSize];
                break;
            case BasicAssetType.Mouth:
                mouths = new BasicAsset[listSize];
                break;
            case BasicAssetType.Hair:
                hairs = new BasicAsset[listSize];
                break;
            case BasicAssetType.FacialMark:
                facialMarks = new BasicAsset[listSize];
                break;
            case BasicAssetType.HeadDecor:
                headDecor = new BasicAsset[listSize];
                break;
            case BasicAssetType.Shirt:
                shirts = new BasicAsset[listSize];
                break;
            case BasicAssetType.Outfit:
                outfits = new BasicAsset[listSize];
                break;
            case BasicAssetType.Pants:
                pants = new BasicAsset[listSize];
                break;
            case BasicAssetType.Socks:
                socks = new BasicAsset[listSize];
                break;
            case BasicAssetType.Shoes:
                shoes = new BasicAsset[listSize];
                break;
            default:
                Debug.LogWarning("Warning: (AvatarCosmeticsList(ResetListWithSize)) was not given " +
                    "the right BasicAssetType for this list type.");
                break;
        }
    }    

    public void SetListItem(BasicAssetType type, BasicAsset asset, int index)
    {
        try
        {
            switch (type)
            {
                case BasicAssetType.Eyebrow:
                    eyebrows[index] = asset;
                    break;
                case BasicAssetType.Eyes:
                    eyes[index] = asset;
                    break;
                case BasicAssetType.Mouth:
                    mouths[index] = asset;
                    break;
                case BasicAssetType.Hair:
                    hairs[index] = asset;
                    break;
                case BasicAssetType.FacialMark:
                    facialMarks[index] = asset;
                    break;
                case BasicAssetType.HeadDecor:
                    headDecor[index] = asset;
                    break;
                case BasicAssetType.Shirt:
                    shirts[index] = asset;
                    break;
                case BasicAssetType.Outfit:
                    outfits[index] = asset;
                    break;
                case BasicAssetType.Pants:
                    pants[index] = asset;
                    break;
                case BasicAssetType.Socks:
                    socks[index] = asset;
                    break;
                case BasicAssetType.Shoes:
                    shoes[index] = asset;
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

    public BasicAsset[] GetList(BasicAssetType type)
    {
        switch (type)
        {
            case BasicAssetType.Eyebrow:
                return eyebrows;
            case BasicAssetType.Eyes:
                return eyes;
            case BasicAssetType.Mouth:
                return mouths;
            case BasicAssetType.Hair:
                return hairs;
            case BasicAssetType.FacialMark:
                return facialMarks;
            case BasicAssetType.HeadDecor:
                return headDecor;
            case BasicAssetType.Shirt:
                return shirts;
            case BasicAssetType.Outfit:
                return outfits;
            case BasicAssetType.Pants:
                return pants;
            case BasicAssetType.Socks:
                return socks;
            case BasicAssetType.Shoes:
                return shoes;
            default:
                Debug.LogWarning("Warning: (AvatarCosmeticsList(GetList)) was not given " +
                    "the right BasicAssetType for this list type.");
                return new BasicAsset[0];
        }
    }
}
