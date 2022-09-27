using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSavesInputControl : MonoBehaviour
{
    public void OnClickBack()
    {
        Debug.Log("Back");
    }
    public void OnClickNewMap()
    {
        Debug.Log("New Map");
    }
    public void OnClickArchives()
    {
        Debug.Log("Archives");
    }
    public void OnClickChangeName()
    {
        Debug.Log("ChangeName");
    }
    public void OnClickOpenScroll(int scrollNumber)
    {
        Debug.Log("Open Scroll " + scrollNumber);
    }
}
