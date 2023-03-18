using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BasicAssetType
{
    // World Objects
    Prop, Building, MiscWorld,
    // Character Creation
    Body,
    Eyebrow, Eyes, Mouth, Hair, FacialMark,
    HeadDecor, Shirt, Outfit, Pants, Socks, Shoes
}
public class BasicAsset : ScriptableObject
{
    public Sprite mainSprite;
    public BasicAssetType assetType;
    public virtual void Init(Sprite sprite, BasicAssetType type) 
    {
        mainSprite = sprite;
        assetType = type;
    }
}
