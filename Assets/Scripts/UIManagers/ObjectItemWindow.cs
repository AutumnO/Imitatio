using UnityEngine;
using UnityEngine.UI;

public class ObjectItemWindow : MonoBehaviour
{
    [SerializeField] private GameObject _objectListItem;
    [SerializeField] private Sprite[] _objectSprites;

    private GameObject[] _listItems;
    private int _itemOffset;
    private int _spacing; //units left and right from item window center to draw list item
    private int _ySpacing; //spacing between top of window and list item


    private void Awake()
    {
        _itemOffset = 3;
        _spacing = 45;
        _ySpacing = 5;
        _listItems = new GameObject[_objectSprites.Length];

        Vector3 pos = new Vector3(0, 0, 0);
        for (int i = 0; i < _objectSprites.Length; i++)
        {
            if (i % _itemOffset == 0)
                pos.x = -_spacing;
            else if (i % _itemOffset == 1)
                pos.x = 0;
            else if (i % _itemOffset == 2)
                pos.x = _spacing;

            pos.y = ((i / _itemOffset) * -_spacing) - _ySpacing;
            _listItems[i] = Instantiate(_objectListItem, transform);
            _listItems[i].GetComponent<Transform>().localPosition = pos;
            _listItems[i].GetComponentsInChildren<Image>()[1].sprite = _objectSprites[i];
            _listItems[i].GetComponentsInChildren<Image>()[1].preserveAspect = true;
        }
    }
}
