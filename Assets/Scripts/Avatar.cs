using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

    private BasicAsset _facialMarks;
    private BasicAsset _eyebrows;
    private BasicAsset _eyes;
    private BasicAsset _mouth;
    private BasicAsset _hair;
    private BasicAsset _headDecor;

    private BasicAsset _body;
    private BasicAsset _clothing;

    public void SetAsset(BasicAsset asset, BasicAssetType type)
    {
        GetAssetFromType(type) = asset;
        UpdateSprites();
    }

    private ref BasicAsset GetAssetFromType(BasicAssetType type)
    {
        switch(type)
        {
            case BasicAssetType.FacialMark:
                return ref _facialMarks;
            case BasicAssetType.Eyebrow:
                return ref _eyebrows;
            case BasicAssetType.Eyes:
                return ref _eyes;
            case BasicAssetType.Mouth:
                return ref _mouth;
            case BasicAssetType.Hair:
                return ref _hair;
            case BasicAssetType.HeadDecor:
                return ref _headDecor;
            case BasicAssetType.Body:
                return ref _body;
            default:
                Debug.LogWarning("Avatar class GetAssetFromType not provided with recognized BasicAssetType");
                return ref _clothing;
        }
    }

    private void UpdateSprites()
    {
        if(_facialMarks != null)
            head.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _facialMarks.mainSprite;
        if(_eyebrows != null)
            head.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = _eyebrows.mainSprite;
        if (_eyes != null)
            head.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = _eyes.mainSprite;
        if (_mouth != null)
            head.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = _mouth.mainSprite;
        if (_hair != null)
            head.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = _hair.mainSprite;
        if (_headDecor != null)
            head.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sprite = _headDecor.mainSprite;

        if (_body != null)
            body.GetComponent<SpriteRenderer>().sprite = _body.mainSprite;
        if (_clothing != null)
            body.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _clothing.mainSprite;
    }
}
