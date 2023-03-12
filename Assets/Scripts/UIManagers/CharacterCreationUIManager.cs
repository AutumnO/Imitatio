using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationUIManager : MonoBehaviour
{
    [SerializeField] private AvatarCosmeticsList _avatarCosmeticsAssets;
    [SerializeField] private GameObject _caroselItemPrefab;

    public AvatarCosmeticsList GetCaroselObjects()
    {
        return _avatarCosmeticsAssets;
    }

    public GameObject GetCaroselItemPrefab()
    {
        return _caroselItemPrefab;
    }
}
