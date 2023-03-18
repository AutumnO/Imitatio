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

    public void Start()
    {
        AddBodyTypeListeners();
    }

    public AvatarCosmeticsList GetCaroselObjects()
    {
        return _avatarCosmeticsAssets;
    }

    public GameObject GetCaroselItemPrefab()
    {
        return _caroselItemPrefab;
    }

    // Listeners
    
    // Add Toggle Listeners
    private void AddBodyTypeListeners()
    {
        Toggle[] toggles = _bodyTypePanel.GetComponentsInChildren<Toggle>();
        foreach(Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { ChangeBody(toggle); });
        }
    }
    private void ChangeBody(Toggle toggle)
    {
        if(toggle.isOn == true)
        {
            BasicAsset asset = toggle.gameObject.GetComponent<AssetComponent>().asset;
            _mainAvatar.GetComponent<Avatar>().SetAsset(asset, BasicAssetType.Body);
        }
    }
}
