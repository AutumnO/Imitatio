using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectSelectionWindow : MonoBehaviour
{
    [SerializeField] protected GameObject _objectListItem;
    [SerializeField] private BasicAssetType _objType;

    protected GridManager _gridManager;
    protected GameObject[] _listItems;

    private WorldObjectData[] _worldObjects;

    private int _itemOffset;
    private int _spacing; //units left and right from item window center to draw list item
    private int _ySpacing; //spacing between top of window and list item


    private void Awake()
    {
        _itemOffset = 3;
        _spacing = 45;
        _ySpacing = 5;
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        BuildItemList(GetItemList());
    }

    public virtual int GetItemList()
    {
        _worldObjects = _gridManager.GetWorldObjects(_objType);
        return _worldObjects.Length;
    }
    public virtual Sprite GetWindowSprite(int index)
    {
        return _worldObjects[index].mainSprite;
    }
    private void BuildItemList(int listLength) 
    {
        _listItems = new GameObject[listLength];
        Vector3 pos;
        for (int i = 0; i < listLength; i++)
        {
            pos = GetNextWindowPosition(i);

            _listItems[i] = Instantiate(_objectListItem, transform);
            _listItems[i].GetComponent<Transform>().localPosition = pos;
            _listItems[i].GetComponentsInChildren<Image>()[1].sprite = GetWindowSprite(i);
            _listItems[i].GetComponentsInChildren<Image>()[1].preserveAspect = true;

            // add onclick event
            AddObjectSelectEvent(_listItems[i], i);
        }
    }
    
    public Vector3 GetNextWindowPosition(int itemNum)
    {
        Vector3 pos = new Vector3(0, 0, 0);

        if (itemNum % _itemOffset == 0)
            pos.x = -_spacing;
        else if (itemNum % _itemOffset == 1)
            pos.x = 0;
        else if (itemNum % _itemOffset == 2)
            pos.x = _spacing;

        pos.y = ((itemNum / _itemOffset) * -_spacing) - _ySpacing;

        return pos;
    }

    // Add PointerClick Event Trigger
    protected void AddObjectSelectEvent(GameObject obj, int dataIndex)
    {
        if (obj.GetComponent<EventTrigger>() == null)
            obj.AddComponent<EventTrigger>();

        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // entry is the thing thats added to the EventTrigger component
        entry.eventID = EventTriggerType.PointerClick;
        SetListener(entry, dataIndex);
        trigger.triggers.Add(entry);
    }
    public virtual void SetListener(EventTrigger.Entry entry, int dataIndex)
    {
        entry.callback.AddListener((func) => { _gridManager.GrabObject(_worldObjects[dataIndex]); });
    }
}
