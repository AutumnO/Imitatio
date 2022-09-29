using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreationPanelManager : MonoBehaviour
{
    [SerializeField] private Transform _panelParent;

    private Transform _panel1;
    private Transform _panel2;
    private Transform _panel3;
    private Transform _panel4;

    private void Awake()
    {
        // find object choice panels
        _panel1 = _panelParent.Find("Panel1");
        _panel2 = _panelParent.Find("Panel2");
        _panel3 = _panelParent.Find("Panel3");
        _panel4 = _panelParent.Find("Panel4");
    }

    // Panel Changes
    public void OnClickChangePanel(string panelName)
    {
        if (panelName == "Panel1")
        {
            _panel1.SetAsLastSibling();
            _panel2.SetSiblingIndex(2);
            _panel3.SetSiblingIndex(1);
        }
        else if (panelName == "Panel2")
        {
            _panel2.SetAsLastSibling();
            _panel1.SetSiblingIndex(2);
            _panel3.SetSiblingIndex(1);
        }
        else if (panelName == "Panel3")
        {
            _panel3.SetAsLastSibling();
            _panel4.SetSiblingIndex(2);
            _panel2.SetSiblingIndex(1);
        }
        else if (panelName == "Panel4")
        {
            _panel4.SetAsLastSibling();
            _panel3.SetSiblingIndex(2);
            _panel2.SetSiblingIndex(1);
        }
    }
}
