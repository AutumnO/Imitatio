using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CASCarosel : MonoBehaviour
{
    // list and object type for this carosel
    [SerializeField] private BasicAssetType _objType;
    private BasicAsset[] _objects;
    protected GameObject[] _listItems;

    protected CharacterCreationUIManager _uiManager;

    private void Awake()
    {
        _uiManager = GameObject.Find("SceneManager").GetComponent<CharacterCreationUIManager>();
        BuildItemList();
    }

    private void BuildItemList()
    {
        _objects = _uiManager.GetCaroselObjects().GetList(_objType);
        _listItems = new GameObject[_objects.Length];
        GameObject caroselItem = _uiManager.GetCaroselItemPrefab();
        for (int i = 0; i < _objects.Length; i++)
        {
            _listItems[i] = Instantiate(caroselItem, transform);
            _listItems[i].GetComponentsInChildren<Image>()[1].sprite = _objects[i].mainSprite;
            _listItems[i].GetComponentsInChildren<Image>()[1].preserveAspect = true;
        }
    }
}
