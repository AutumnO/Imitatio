using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUIManager : MonoBehaviour
{
    [SerializeField] private AvatarCosmeticsList _avatarCosmeticsAssets;
    [SerializeField] private GameObject _caroselItemPrefab;
    [SerializeField] private GameObject _mainAvatar;
    [SerializeField] private GameObject _nameInputFieldObj;

    [SerializeField] private Button _addFamilyButton;


    // toggle groups
    [SerializeField] private GameObject _genderToggleGroupObj;
    [SerializeField] private GameObject _ageToggleGroupObj;
    [SerializeField] private GameObject _skinColorToggleGroupObj;
    [SerializeField] private GameObject _bodyTypePanel;
    [SerializeField] private GameObject _eyebrowsToggleGroupObj;
    [SerializeField] private GameObject _eyesToggleGroupObj;
    [SerializeField] private GameObject _mouthToggleGroupObj;

    public void Start()
    {
        AddAvatarNameListener();
        AddAvatarInfoListener(_genderToggleGroupObj, "gender");
        AddAvatarInfoListener(_ageToggleGroupObj, "age");
        AddAvatarInfoListener(_skinColorToggleGroupObj, "skin");
        AddAvatarListener(_bodyTypePanel, BasicAssetType.Body);
        AddAvatarListener(_eyebrowsToggleGroupObj, BasicAssetType.Eyebrow);
        AddAvatarListener(_eyesToggleGroupObj, BasicAssetType.Eyes);
        AddAvatarListener(_mouthToggleGroupObj, BasicAssetType.Mouth);

        AddRandomizeListener(_addFamilyButton);
    }

    public AvatarCosmeticsList GetCaroselObjects()
    {
        return _avatarCosmeticsAssets;
    }

    public GameObject GetCaroselItemPrefab()
    {
        return _caroselItemPrefab;
    }
    

    // CHANGING THE AVATAR BASED ON USER SELECTIONS

    /// <summary>
    /// Adds an onEndEdit event listener to change the main avatar's name when the user 
    /// changes the name input field.
    /// </summary>
    private void AddAvatarNameListener()
    {
        Avatar avatar = _mainAvatar.GetComponent<Avatar>();
        TMP_InputField nameInputField = _nameInputFieldObj.GetComponent<TMP_InputField>();
        nameInputField.onEndEdit.AddListener(avatar.SetName);
    }

    /// <summary>
    /// Adds event listeners to each toggle in the toggle group component on the provided object that
    /// change information on the main avatar when selected.
    /// </summary>
    /// <param name="toggleGroupObj">The game object that has a Toggle Group component.</param>
    /// <param name="type">The type of information associated with the toggles. Valid values: gender, age, skin</param>
    private void AddAvatarInfoListener(GameObject toggleGroupObj, string type)
    {
        Avatar avatar = _mainAvatar.GetComponent<Avatar>();
        Toggle[] toggles = toggleGroupObj.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (type == "gender")
                toggle.onValueChanged.AddListener(delegate { avatar.SetGender(toggle.gameObject.name); });
            else if (type == "age")
                toggle.onValueChanged.AddListener(delegate { avatar.SetAge(toggle.gameObject.name); });
            else if (type == "skin")
                toggle.onValueChanged.AddListener(delegate { avatar.SetSkin(Int32.Parse(toggle.gameObject.name.Substring(4))); });
            else
                Debug.LogWarning("CharacterCreationUIManager AddAvatarInfoListener method couldn't parse type parameter.");
        }
    }

    /// <summary>
    /// Adds event listeners to each toggle in the toggle group component on the provided object that
    /// change visual assets on the main avatar when selected.
    /// </summary>
    /// <param name="toggleGroupObj">The game object that has a Toggle Group component.</param>
    /// <param name="type">The type of asset associated with the toggles.</param>
    private void AddAvatarListener(GameObject toggleGroupObj, BasicAssetType type)
    {
        Toggle[] toggles = toggleGroupObj.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { ChangeFeature(toggle, type); });
        }
    }
    /// <summary>
    /// Sets the main avatar's visual features to a new value. Designed for an onValueChanged toggle listener.
    /// </summary>
    /// <param name="toggle">The toggle that was triggered.</param>
    /// <param name="type">The type of asset associated with the trigger.</param>
    private void ChangeFeature(Toggle toggle, BasicAssetType type)
    {
        if(toggle.isOn == true)
        {
            BasicAsset asset = toggle.gameObject.GetComponent<AssetComponent>().asset;
            _mainAvatar.GetComponent<Avatar>().SetAsset(asset, type);
        }
    }

    private void AddRandomizeListener(Button addFamilyButton)
    {
        Avatar avatar = _mainAvatar.GetComponent<Avatar>();
        AvatarRandomizer rand = gameObject.GetComponent<AvatarRandomizer>();
        addFamilyButton.onClick.AddListener(delegate { rand.RandomizeAvatar(avatar, true); });
    }
}
