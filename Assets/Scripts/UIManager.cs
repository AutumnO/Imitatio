using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // MAP CREATION
    
    // add limit to panning/zooming so one doesn't get too far away or close to grid

    public void OnClickBack()
    {
        Debug.Log("Back Clicked");
    }

    public void OnClickSaveExit()
    {
        Debug.Log("Save & Exit Clicked");
    }

    // Compass
    public void OnClickCompassZoomIn()
    {
        Debug.Log("Zoom In");
    }
    public void OnClickCompassZoomOut()
    {
        Debug.Log("Zoom Out");
    }
    public void OnClickCompassNorth()
    {
        Debug.Log("North");
    }
    public void OnClickCompassEast()
    {
        Debug.Log("East");
    }
    public void OnClickCompassSouth()
    {
        Debug.Log("South");
    }
    public void OnClickCompassWest()
    {
        Debug.Log("West");
    }

    // Tools
    public void OnClickToolsHand()
    {
        Debug.Log("Hand");
    }
    public void OnClickToolsHammer()
    {
        Debug.Log("Hammer");
    }
    public void OnClickToolsLighting()
    {
        Debug.Log("Lighting");
    }
    public void OnClickToolsCopy()
    {
        Debug.Log("Copy");
    }
    public void OnClickToolsUndo()
    {
        Debug.Log("Undo");
    }
    public void OnClickToolsRedo()
    {
        Debug.Log("Redo");
    }
}
