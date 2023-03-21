using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUIManager : MonoBehaviour
{
    [SerializeField] private AvatarCosmeticsList _avatarCosmeticsAssets;
    [SerializeField] private GameObject _caroselItemPrefab;
    [SerializeField] private GameObject _mainAvatar;
    [SerializeField] private GameObject _bodyTypePanel;
    [SerializeField] private GameObject _eyebrowsToggleGroupObj;
    [SerializeField] private GameObject _eyesToggleGroupObj;
    [SerializeField] private GameObject _mouthToggleGroupObj;

    public void Start()
    {
        AddAvatarListener(_bodyTypePanel, BasicAssetType.Body);
        AddAvatarListener(_eyebrowsToggleGroupObj, BasicAssetType.Eyebrow);
        AddAvatarListener(_eyesToggleGroupObj, BasicAssetType.Eyes);
        AddAvatarListener(_mouthToggleGroupObj, BasicAssetType.Mouth);
    }

    public AvatarCosmeticsList GetCaroselObjects()
    {
        return _avatarCosmeticsAssets;
    }

    public GameObject GetCaroselItemPrefab()
    {
        return _caroselItemPrefab;
    }
    
    // Add listeners to toggle groups to change features on the main avatar when things are selected

    private void AddAvatarListener(GameObject toggleGroupObj, BasicAssetType type)
    {
        Toggle[] toggles = toggleGroupObj.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { ChangeFeature(toggle, type); });
        }
    }

    private void ChangeFeature(Toggle toggle, BasicAssetType type)
    {
        if(toggle.isOn == true)
        {
            BasicAsset asset = toggle.gameObject.GetComponent<AssetComponent>().asset;
            _mainAvatar.GetComponent<Avatar>().SetAsset(asset, type);
        }
    }
}
