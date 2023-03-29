using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRandomizer : MonoBehaviour
{
    // avatar feature databases
    [SerializeField] private AvatarNamesDB _names;
    [SerializeField] private AvatarCosmeticsList _cosmetics;

    // character creation panels
    [SerializeField] private GameObject _mainLeftPanel;

    public void RandomizeAvatar(Avatar avatar, bool isMainAvatar)
    {
        // name & gender
        if (Random.Range(0, 2) == 0)
        {
            avatar.SetGender("female");
            avatar.SetName(_names.femaleNames[Random.Range(0, _names.femaleNames.Count)].avatarName);
        }
        else
        {
            avatar.SetGender("male");
            avatar.SetName(_names.maleNames[Random.Range(0, _names.maleNames.Count)].avatarName);
        }

        // body features
        // TODO: skin color
        avatar.SetAsset(_cosmetics.bodies[Random.Range(0, _cosmetics.bodies.Length)], BasicAssetType.Body);

        avatar.SetAsset(_cosmetics.eyebrows[Random.Range(0, _cosmetics.eyebrows.Length)], BasicAssetType.Eyebrow);
        avatar.SetAsset(_cosmetics.hairs[Random.Range(0, _cosmetics.hairs.Length)], BasicAssetType.Hair);

        avatar.SetAsset(_cosmetics.eyes[Random.Range(0, _cosmetics.eyes.Length)], BasicAssetType.Eyes);
        avatar.SetAsset(_cosmetics.mouths[Random.Range(0, _cosmetics.mouths.Length)], BasicAssetType.Mouth);
        //avatar.SetAsset(_cosmetics.facialMarks[Random.Range(0, _cosmetics.facialMarks.Length)], BasicAssetType.FacialMark);

        // clothing
        /*
        avatar.SetAsset(_cosmetics.headDecor[Random.Range(0, _cosmetics.headDecor.Length)], BasicAssetType.HeadDecor);
        
        if (Random.Range(0, 5) < 4) // 15% chance to wear an outfit rather than shirt/pants
        {
            avatar.SetAsset(_cosmetics.shirts[Random.Range(0, _cosmetics.shirts.Length)], BasicAssetType.Shirt);
            avatar.SetAsset(_cosmetics.pants[Random.Range(0, _cosmetics.pants.Length)], BasicAssetType.Pants);
        }
        else
            avatar.SetAsset(_cosmetics.outfits[Random.Range(0, _cosmetics.outfits.Length)], BasicAssetType.Outfit);
        
        avatar.SetAsset(_cosmetics.socks[Random.Range(0, _cosmetics.socks.Length)], BasicAssetType.Socks);
        avatar.SetAsset(_cosmetics.shoes[Random.Range(0, _cosmetics.shoes.Length)], BasicAssetType.Shoes);
        */

        if (!isMainAvatar)
        {
            // relationship
            // age
        }
        else
        {
            // select the chosen catagories in character creation panels
        }
    }
}
